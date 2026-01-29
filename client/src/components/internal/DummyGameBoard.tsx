'use client'

import React from 'react'
import { RoundedBox, Text } from '@react-three/drei'
import GameBoard, { StackEffect, Step } from '../GameBoard'

const CARD_WIDTH = 0.7
const CARD_HEIGHT = 1
const CARD_DEPTH = 0.02

// Card data type
type CardData = {
  id: number
  color: string
  label: string
  description?: string
  attack?: number
  health?: number
}

// Sample card data for the demo
const samplePlayerHand: CardData[] = [
  { id: 1, color: '#c9a959', label: 'Knight', description: 'A brave warrior who fights for honor.', attack: 4, health: 5 },
  { id: 2, color: '#6b9bc3', label: 'Mage', description: 'Wields powerful arcane magic.', attack: 3, health: 3 },
  { id: 3, color: '#c75d5d', label: 'Archer', description: 'Strikes from a distance with deadly precision.', attack: 5, health: 2 },
  { id: 4, color: '#7bc275', label: 'Healer', description: 'Restores health to wounded allies.', attack: 1, health: 4 },
  { id: 5, color: '#9b7bc2', label: 'Rogue', description: 'Sneaky assassin who deals double damage from stealth.', attack: 6, health: 2 },
]

const sampleOpponentHand: CardData[] = [
  { id: 101, color: '#555', label: '?', description: 'Unknown card' },
  { id: 102, color: '#555', label: '?', description: 'Unknown card' },
  { id: 103, color: '#555', label: '?', description: 'Unknown card' },
  { id: 104, color: '#555', label: '?', description: 'Unknown card' },
]

const samplePlayerBattlefield: CardData[] = [
  { id: 11, color: '#c9a959', label: 'Knight', description: 'A brave warrior who fights for honor.', attack: 4, health: 5 },
  { id: 12, color: '#6b9bc3', label: 'Mage', description: 'Wields powerful arcane magic.', attack: 3, health: 3 },
  { id: 13, color: '#7bc275', label: 'Healer', description: 'Restores health to wounded allies.', attack: 1, health: 4 },
]

const sampleOpponentBattlefield: CardData[] = [
  { id: 111, color: '#c75d5d', label: 'Dragon', description: 'Ancient beast that breathes fire.', attack: 8, health: 8 },
  { id: 112, color: '#9b7bc2', label: 'Demon', description: 'Creature from the underworld.', attack: 6, health: 5 },
]

const samplePlayerDeck: CardData[] = [
  { id: 201, color: '#c9a959', label: 'Knight', description: 'A brave warrior who fights for honor.', attack: 4, health: 5 },
  { id: 202, color: '#6b9bc3', label: 'Mage', description: 'Wields powerful arcane magic.', attack: 3, health: 3 },
  { id: 203, color: '#c75d5d', label: 'Archer', description: 'Strikes from a distance with deadly precision.', attack: 5, health: 2 },
  { id: 204, color: '#7bc275', label: 'Healer', description: 'Restores health to wounded allies.', attack: 1, health: 4 },
  { id: 205, color: '#9b7bc2', label: 'Rogue', description: 'Sneaky assassin who deals double damage from stealth.', attack: 6, health: 2 },
  { id: 206, color: '#c9a959', label: 'Knight', description: 'A brave warrior who fights for honor.', attack: 4, health: 5 },
  { id: 207, color: '#6b9bc3', label: 'Mage', description: 'Wields powerful arcane magic.', attack: 3, health: 3 },
  { id: 208, color: '#8b6914', label: 'Paladin', description: 'Holy warrior blessed with divine power.', attack: 5, health: 6 },
  { id: 209, color: '#4a6fa5', label: 'Wizard', description: 'Master of elemental spells.', attack: 4, health: 2 },
  { id: 210, color: '#7b4a7c', label: 'Warlock', description: 'Dark mage who bargains with demons.', attack: 7, health: 3 },
]

const sampleOpponentDeck: CardData[] = [
  { id: 301, color: '#c75d5d', label: 'Dragon', description: 'Ancient beast that breathes fire.', attack: 8, health: 8 },
  { id: 302, color: '#9b7bc2', label: 'Demon', description: 'Creature from the underworld.', attack: 6, health: 5 },
  { id: 303, color: '#5d7c5d', label: 'Troll', description: 'Regenerates health each turn.', attack: 4, health: 7 },
  { id: 304, color: '#7c6a5d', label: 'Golem', description: 'Construct of stone and magic.', attack: 3, health: 10 },
  { id: 305, color: '#5d5d7c', label: 'Specter', description: 'Ghostly apparition that ignores armor.', attack: 5, health: 3 },
  { id: 306, color: '#c75d5d', label: 'Dragon', description: 'Ancient beast that breathes fire.', attack: 8, health: 8 },
  { id: 307, color: '#9b7bc2', label: 'Demon', description: 'Creature from the underworld.', attack: 6, health: 5 },
  { id: 308, color: '#8b4513', label: 'Ogre', description: 'Brutish creature with immense strength.', attack: 7, health: 6 },
]

const samplePlayerGraveyard: CardData[] = [
  { id: 401, color: '#888', label: 'Soldier', description: 'A fallen warrior.', attack: 2, health: 2 },
  { id: 402, color: '#666', label: 'Scout', description: 'Reconnaissance unit, now deceased.', attack: 1, health: 1 },
  { id: 403, color: '#777', label: 'Squire', description: 'Knight in training, met an early end.', attack: 2, health: 3 },
]

const sampleOpponentGraveyard: CardData[] = [
  { id: 501, color: '#555', label: 'Imp', description: 'Minor demon, easily dispatched.', attack: 1, health: 1 },
  { id: 502, color: '#666', label: 'Skeleton', description: 'Reanimated bones, now truly dead.', attack: 2, health: 1 },
  { id: 503, color: '#777', label: 'Zombie', description: 'Undead creature, returned to rest.', attack: 3, health: 2 },
  { id: 504, color: '#888', label: 'Ghoul', description: 'Flesh-eating monster, destroyed.', attack: 4, health: 3 },
  { id: 505, color: '#999', label: 'Wraith', description: 'Vengeful spirit, banished.', attack: 5, health: 2 },
]

const samplePlayerExile: CardData[] = [
  { id: 601, color: '#4a4a5a', label: 'Banished', description: 'Removed from the game entirely.', attack: 3, health: 3 },
]

const sampleOpponentExile: CardData[] = [
  { id: 701, color: '#5a4a4a', label: 'Exiled', description: 'Cast into the void.', attack: 4, health: 2 },
  { id: 702, color: '#4a5a4a', label: 'Erased', description: 'Wiped from existence.', attack: 2, health: 5 },
]

const samplePlayerPrizeCards: CardData[] = [
  { id: 801, color: '#5a5a6a', label: 'Prize 1', description: 'A hidden prize card.' },
  { id: 802, color: '#5a5a6a', label: 'Prize 2', description: 'A hidden prize card.' },
  { id: 803, color: '#5a5a6a', label: 'Prize 3', description: 'A hidden prize card.' },
  { id: 804, color: '#5a5a6a', label: 'Prize 4', description: 'A hidden prize card.' },
]

const sampleOpponentPrizeCards: CardData[] = [
  { id: 901, color: '#6a5a5a', label: 'Prize 1', description: 'A hidden prize card.' },
  { id: 902, color: '#6a5a5a', label: 'Prize 2', description: 'A hidden prize card.' },
  { id: 903, color: '#6a5a5a', label: 'Prize 3', description: 'A hidden prize card.' },
  { id: 904, color: '#6a5a5a', label: 'Prize 4', description: 'A hidden prize card.' },
  { id: 905, color: '#6a5a5a', label: 'Prize 5', description: 'A hidden prize card.' },
  { id: 906, color: '#6a5a5a', label: 'Prize 6', description: 'A hidden prize card.' },
]

const samplePlayerSideHand: CardData[] = [
  { id: 1001, color: '#d4a574', label: 'Reserve', description: 'A card held in reserve for special tactics.', attack: 3, health: 4 },
  { id: 1002, color: '#74a5d4', label: 'Backup', description: 'Emergency reinforcement unit.', attack: 2, health: 3 },
  { id: 1003, color: '#a574d4', label: 'Secret', description: 'A hidden ace up your sleeve.', attack: 4, health: 2 },
]

const sampleOpponentSideHand: CardData[] = [
  { id: 1101, color: '#555', label: '?', description: 'Unknown reserve card.' },
  { id: 1102, color: '#555', label: '?', description: 'Unknown reserve card.' },
]

// Sample steps for testing
const sampleSteps: Step[] = [
  { id: 'untap', label: 'Untap', isActive: false },
  { id: 'upkeep', label: 'Upkeep', isActive: false },
  { id: 'draw', label: 'Draw', isActive: true },
  { id: 'main', label: 'Main', isActive: false },
  { id: 'combat', label: 'Combat', isActive: false },
]

// Sample stack for testing
const sampleStack: StackEffect<CardData>[] = [
  {
    card: { id: 601, color: '#e8a040', label: 'Fireball', description: 'Deals 5 damage to target creature.', attack: 5 },
    targetCardIds: ['111'],
  },
  {
    card: { id: 602, color: '#40a0e8', label: 'Counterspell', description: 'Counter target spell.', attack: undefined, health: undefined },
    targetCardIds: ['601'],
  },
]

type CardProps = {
  color?: string
  faceDown?: boolean
  label?: string
  rotation?: [number, number, number]
  scale?: number
}

// Card colors for different zones/purposes
const COLORS = {
  cardFront: '#e8e0d5',
  cardBack: '#2a4858',
  cardBorder: '#1a1a1a',
  deck: '#1e3a4c',
  graveyard: '#3d2c3d',
  battlefield: '#2d5a3d',
  hand: '#4a3728',
}

// Purely visual card component - no interactions
function Card({
  color = COLORS.cardFront,
  faceDown = false,
  label,
  rotation = [0, 0, 0],
  scale = 1,
}: CardProps) {
  return (
    <group rotation={rotation} scale={scale}>
      <RoundedBox args={[CARD_WIDTH, CARD_HEIGHT, CARD_DEPTH]} radius={0.03} smoothness={4}>
        <meshStandardMaterial color={faceDown ? COLORS.cardBack : color} />
      </RoundedBox>
      {/* Card border/frame */}
      <RoundedBox
        args={[CARD_WIDTH * 0.92, CARD_HEIGHT * 0.92, CARD_DEPTH + 0.005]}
        radius={0.02}
        smoothness={4}
        position={[0, 0, 0.001]}
      >
        <meshStandardMaterial color={faceDown ? COLORS.cardBack : color} />
      </RoundedBox>
      {label && !faceDown && (
        <Text
          position={[0, 0, CARD_DEPTH / 2 + 0.01]}
          fontSize={0.12}
          color="#333"
          anchorX="center"
          anchorY="middle"
        >
          {label}
        </Text>
      )}
    </group>
  )
}

// Render function for CardData cards - purely visual (3D mesh version)
function renderSampleCardMesh(card: CardData): React.ReactNode {
  return (
    <Card
      faceDown={false}
      color={card.color}
      label={card.label}
    />
  )
}

function getCardDataId(card: CardData): number {
  return card.id
}

// Sample message component
function SampleMessage({ onClose }: { onClose: () => void }) {
  return (
    <div
      style={{
        backgroundColor: 'rgba(42, 42, 58, 0.9)',
        borderRadius: '0 0 12px 12px',
        padding: '12px 24px',
        boxShadow: '0 4px 16px rgba(0, 0, 0, 0.4)',
        border: '1px solid #4a4a6a',
        borderTop: 'none',
        fontFamily: 'system-ui, sans-serif',
        color: '#eee',
        textAlign: 'center',
      }}
    >
      <span style={{ fontSize: '16px', fontWeight: 'bold' }}>Your Turn</span>
      <span style={{ margin: '0 12px', color: '#666' }}>|</span>
      <span style={{ fontSize: '14px', color: '#aaa' }}>Select a card to play</span>
      <button onClick={onClose}>Close</button>
    </div>
  )
}

// Sample bottom bar component
function SampleBottomBar() {
  return (
    <div
      style={{
        backgroundColor: 'rgba(30, 30, 40, 0.95)',
        padding: '8px 16px',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        gap: '16px',
        borderTop: '1px solid #4a4a6a',
        fontFamily: 'system-ui, sans-serif',
      }}
    >
      <button
        style={{
          padding: '8px 16px',
          fontSize: '13px',
          fontWeight: 'bold',
          backgroundColor: '#4a6a4a',
          color: 'white',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer',
        }}
      >
        End Turn
      </button>
      <button
        style={{
          padding: '8px 16px',
          fontSize: '13px',
          backgroundColor: '#3a3a4a',
          color: '#aaa',
          border: '1px solid #5a5a6a',
          borderRadius: '4px',
          cursor: 'pointer',
        }}
      >
        Settings
      </button>
      <span style={{ color: '#666', fontSize: '12px' }}>Mana: 5/10</span>
    </div>
  )
}

// Sample dialog component
function SampleDialog({ onClose }: { onClose: () => void }) {
  return (
    <div
      style={{
        backgroundColor: '#2a2a3a',
        borderRadius: '12px',
        padding: '24px',
        minWidth: '300px',
        boxShadow: '0 8px 32px rgba(0, 0, 0, 0.5)',
        border: '2px solid #4a4a6a',
        fontFamily: 'system-ui, sans-serif',
        color: '#eee',
      }}
    >
      <h2 style={{ margin: '0 0 16px 0', fontSize: '20px' }}>Sample Dialog</h2>
      <p style={{ margin: '0 0 20px 0', color: '#aaa' }}>
        This is a custom dialog rendered via renderDialog prop.
      </p>
      <button
        onClick={onClose}
        style={{
          padding: '10px 20px',
          fontSize: '14px',
          fontWeight: 'bold',
          backgroundColor: '#5a7a9a',
          color: 'white',
          border: 'none',
          borderRadius: '6px',
          cursor: 'pointer',
        }}
      >
        Close Dialog
      </button>
    </div>
  )
}

function HtmlCard({ card: previewCard }: { card: CardData, faceDown: boolean }): React.ReactNode {
  return (
      <div
        style={{
          width: '180px',
          height: '250px',
          backgroundColor: previewCard.color,
          borderRadius: '12px',
          border: '3px solid #1a1a1a',
          boxShadow: '0 10px 40px rgba(0,0,0,0.5)',
          display: 'flex',
          flexDirection: 'column',
          padding: '10px',
          color: '#333',
          fontFamily: 'system-ui, sans-serif',
        }}
      >
        {/* Card name */}
        <div
          style={{
            fontSize: '16px',
            fontWeight: 'bold',
            textAlign: 'center',
            marginBottom: '6px',
            padding: '4px',
            backgroundColor: 'rgba(255,255,255,0.3)',
            borderRadius: '4px',
          }}
        >
          {previewCard.label}
        </div>

        {/* Card art placeholder */}
        <div
          style={{
            flex: 1,
            backgroundColor: 'rgba(0,0,0,0.1)',
            borderRadius: '4px',
            marginBottom: '6px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            fontSize: '40px',
          }}
        >
          {previewCard.label}
        </div>

        {/* Description */}
        <div
          style={{
            fontSize: '11px',
            textAlign: 'center',
            marginBottom: '6px',
            padding: '4px',
            backgroundColor: 'rgba(255,255,255,0.2)',
            borderRadius: '4px',
            minHeight: '32px',
          }}
        >
          {previewCard.description || 'No description'}
        </div>

        {/* Stats */}
        {(previewCard.attack !== undefined || previewCard.health !== undefined) && (
          <div
            style={{
              display: 'flex',
              justifyContent: 'space-between',
              fontSize: '14px',
              fontWeight: 'bold',
            }}
          >
            <div
              style={{
                backgroundColor: '#c75d5d',
                color: 'white',
                padding: '3px 10px',
                borderRadius: '4px',
              }}
            >
              ⚔ {previewCard.attack ?? '-'}
            </div>
            <div
              style={{
                backgroundColor: '#5dc75d',
                color: 'white',
                padding: '3px 10px',
                borderRadius: '4px',
              }}
            >
              ❤ {previewCard.health ?? '-'}
            </div>
          </div>
        )}
      </div>
  )
}

export default function DummyGameBoard() {
  const [showDialog, setShowDialog] = React.useState(true)
  const [stack, setStack] = React.useState<StackEffect<CardData>[] | null>(sampleStack);

  return (
    <GameBoard<CardData>
      yourHand={samplePlayerHand}
      yourSideHand={samplePlayerSideHand}
      yourBattlefield={samplePlayerBattlefield}
      yourDeck={samplePlayerDeck}
      yourGraveyard={samplePlayerGraveyard}
      yourExile={samplePlayerExile}
      yourPrizeCards={samplePlayerPrizeCards}
      yourHealth={25}
      opponentHand={sampleOpponentHand}
      opponentSideHand={sampleOpponentSideHand}
      opponentBattlefield={sampleOpponentBattlefield}
      opponentDeck={sampleOpponentDeck}
      opponentGraveyard={sampleOpponentGraveyard}
      opponentsExile={sampleOpponentExile}
      opponentsPrizeCards={sampleOpponentPrizeCards}
      opponentHealth={18}
      renderCardMesh={renderSampleCardMesh}
      renderHtmlCard={(card: CardData) => <HtmlCard card={card} faceDown={false} />}
      renderEmptySlot={() => <Card faceDown />}
      getCardId={getCardDataId}
      onExileClick={(isOpponent) => console.log('Exile clicked:', isOpponent ? 'opponent' : 'player')}
      onPrizeCardsClick={(isOpponent) => console.log('Prize cards clicked:', isOpponent ? 'opponent' : 'player')}
      onSideHandClick={() => console.log('Side hand clicked')}
      stack={stack}
      stackButton={() => (
        <button
          onClick={() => console.log('Resolve clicked!')}
          style={{
            padding: '6px 16px',
            fontSize: '12px',
            fontWeight: 'bold',
            backgroundColor: '#6a4a8a',
            color: 'white',
            border: '2px solid #8a6aaa',
            borderRadius: '6px',
            cursor: 'pointer',
          }}
        >
          Resolve
        </button>
      )}
      steps={sampleSteps}
      renderMessage={() => <SampleMessage onClose={() => setStack(null)} />}
      renderBottomBar={() => <SampleBottomBar />}
      renderDialog={() =>
        showDialog ? <SampleDialog onClose={() => setShowDialog(false)} /> : null
      }
    />
  );
}