import { useFrame } from '@react-three/fiber'
import { useCardPositions } from './useCardPositions'
import { useRef, useMemo } from 'react'
import * as THREE from 'three'

type TargetArrow3DProps = {
  fromCardId: string
  toCardId: string
  color?: string
}

// Shader for animated gradient along the tube
const fluxShader = {
  vertexShader: `
    varying vec2 vUv;
    void main() {
      vUv = uv;
      gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }
  `,
  fragmentShader: `
    uniform vec3 color;
    uniform float time;
    varying vec2 vUv;

    void main() {
      // Create flowing wave pattern along the tube (vUv.x is along the curve)
      float wave = sin((vUv.x - time) * 6.28318 * 2.0) * 0.5 + 0.5;

      // Brighter toward the target (higher vUv.x)
      float gradient = vUv.x;

      // Combine: base brightness + animated wave
      float brightness = 0.4 + gradient * 0.3 + wave * 0.3;

      // Fade out at edges of tube (vUv.y is around the circumference)
      float edgeFade = 1.0 - abs(vUv.y - 0.5) * 1.5;
      edgeFade = clamp(edgeFade, 0.0, 1.0);

      // Final color with glow
      vec3 finalColor = color * brightness * 1.2;
      float alpha = brightness * edgeFade * 0.85;

      gl_FragColor = vec4(finalColor, alpha);
    }
  `,
}

export default function TargetArrow3D({
  fromCardId,
  toCardId,
  color = '#88ccff',
}: TargetArrow3DProps) {
  const { getPosition } = useCardPositions()
  const meshRef = useRef<THREE.Mesh>(null)
  const materialRef = useRef<THREE.ShaderMaterial>(null)
  const geometryRef = useRef<THREE.TubeGeometry | null>(null)

  // Parse color once
  const colorVec = useMemo(() => new THREE.Color(color), [color])

  // Create uniforms
  const uniforms = useMemo(
    () => ({
      color: { value: colorVec },
      time: { value: 0 },
    }),
    [colorVec]
  )

  // Track if we have valid positions
  const hasPositions = useRef(false)

  // Update curve and animate each frame
  useFrame((_, delta) => {
    const from = getPosition(fromCardId)
    const to = getPosition(toCardId)

    if (from && to && meshRef.current) {
      // Calculate direction between cards (in XY plane)
      const direction = new THREE.Vector3().subVectors(to, from)
      direction.z = 0 // Keep direction in XY plane
      const distance = direction.length()
      direction.normalize()

      // Card edge offset (approximate card radius)
      const cardOffset = 0.6

      // Start/end positions offset to card edges (slightly above cards)
      const start = from.clone()
      start.add(direction.clone().multiplyScalar(cardOffset))
      start.z += 0.1

      const end = to.clone()
      end.sub(direction.clone().multiplyScalar(cardOffset))
      end.z += 0.1

      // Control point: midpoint raised in Z for arc effect
      // Scale arc height based on distance
      const arcHeight = Math.min(1.5, Math.max(0.5, distance * 0.3))
      const mid = new THREE.Vector3().lerpVectors(start, end, 0.5)
      mid.z += arcHeight

      // Create quadratic bezier curve
      const curve = new THREE.QuadraticBezierCurve3(start, mid, end)

      // Update or create tube geometry
      if (geometryRef.current) {
        geometryRef.current.dispose()
      }
      geometryRef.current = new THREE.TubeGeometry(curve, 32, 0.04, 8, false)
      meshRef.current.geometry = geometryRef.current

      hasPositions.current = true

      // Animate time uniform for flowing effect
      if (materialRef.current) {
        materialRef.current.uniforms.time.value += delta * 0.8
      }
    } else {
      hasPositions.current = false
    }
  })

  return (
    <mesh ref={meshRef} visible={hasPositions.current}>
      <tubeGeometry args={[new THREE.QuadraticBezierCurve3(
        new THREE.Vector3(0, 0, 0),
        new THREE.Vector3(0, 0, 1),
        new THREE.Vector3(0, 0, 2)
      ), 32, 0.04, 8, false]} />
      <shaderMaterial
        ref={materialRef}
        uniforms={uniforms}
        vertexShader={fluxShader.vertexShader}
        fragmentShader={fluxShader.fragmentShader}
        transparent
        side={THREE.DoubleSide}
        depthWrite={false}
      />
    </mesh>
  )
}
