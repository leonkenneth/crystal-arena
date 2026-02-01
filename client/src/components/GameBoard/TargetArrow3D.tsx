import { useFrame } from '@react-three/fiber'
import { Line } from '@react-three/drei'
import { useCardPositions } from './useCardPositions'
import { useRef, useState } from 'react'
import * as THREE from 'three'

type TargetArrow3DProps = {
  fromCardId: string
  toCardId: string
  color?: string
}

export default function TargetArrow3D({
  fromCardId,
  toCardId,
  color = '#4488ff',
}: TargetArrow3DProps) {
  const { getPosition } = useCardPositions()
  const [points, setPoints] = useState<[THREE.Vector3, THREE.Vector3] | null>(null)

  // Update line positions each frame
  useFrame(() => {
    const from = getPosition(fromCardId)
    const to = getPosition(toCardId)

    if (from && to) {
      // Offset the line slightly above the cards
      const fromPoint = from.clone()
      fromPoint.z += 0.1
      const toPoint = to.clone()
      toPoint.z += 0.1

      setPoints([fromPoint, toPoint])
    } else {
      setPoints(null)
    }
  })

  if (!points) return null

  return (
    <Line
      points={points}
      color={color}
      lineWidth={3}
      transparent
      opacity={0.8}
    />
  )
}
