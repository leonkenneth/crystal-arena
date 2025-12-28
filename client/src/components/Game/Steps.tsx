import { StepState } from "@/types";
import { Steps as StepsComponent, Box, Text } from "@chakra-ui/react";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";

const nonCombatStepsToDisplay = ["Draw", "1st main", "Beg. of combat", "2nd main", "End of turn"];
const combatSteps = ["Dec. attackers", "Dec. blockers", "Combat damage"];
const stepsToDisplayInCombat = ["Dec. attackers", "Dec. blockers", "Combat damage"];

const stepsInfo: Record<string, { name: string; shortName: string; indicator: string }> = {
  Draw: {
    name: "Draw",
    shortName: "Drw",
    indicator: "1",
  },
  "1st main": {
    name: "1st main",
    shortName: "1st",
    indicator: "2",
  },
  "Beg. of combat": {
    name: "Combat",
    shortName: "Cmb",
    indicator: "3",
  },
  "Dec. attackers": {
    name: "Dec. attackers",
    shortName: "Att",
    indicator: "3A",
  },
  "Dec. blockers": {
    name: "Dec. blockers",
    shortName: "Blck",
    indicator: "3B",
  },
  "Combat damage": {
    name: "Combat damage",
    shortName: "Dmg",
    indicator: "3C",
  },
  "2nd main": {
    name: "2nd main",
    shortName: "2nd",
    indicator: "4",
  },
  "End of turn": {
    name: "End of turn",
    shortName: "End",
    indicator: "5",
  },
};

function isCombatStep(step: StepState) {
  return combatSteps.includes(step.name);
}

export default function Steps() {
  const { gameState } = useLoadedGameContext();
  const steps = gameState.screen.steps;
  const isYourTurn = gameState.screen.you.isActive;
  const activeStep = steps.steps.find((step) => step.isCurrent);
  const activeStepIsCombatStep = activeStep && isCombatStep(activeStep);

  let stepsToDisplay = [];
  if (activeStepIsCombatStep) {
    stepsToDisplay = steps.steps.filter((step) => stepsToDisplayInCombat.includes(step.name));
  } else {
    stepsToDisplay = steps.steps.filter((step) => nonCombatStepsToDisplay.includes(step.name));
  }
  const activeStepIndex = stepsToDisplay.findIndex((step) => step.isCurrent);

  return (
    <StepsComponent.Root
      step={stepsToDisplay.length}
      size="sm"
      variant="subtle"
      colorPalette="cyan"
    >
      <StepsComponent.List gap={{ base: 1, md: 2 }}>
        {stepsToDisplay.map((step, index) => {
          const stepInfo = stepsInfo[step.name];
          return (
            <StepsComponent.Item key={step.oid.oid} index={index}>
              <StepsComponent.Indicator
                borderColor="fg.muted"
                border={activeStepIndex === index ? "1px solid" : "none"}
              >
                <Text hideFrom="sm">{stepInfo.shortName}</Text>
                <Text hideBelow="sm">{stepInfo.indicator}</Text>
              </StepsComponent.Indicator>
              <Box>
                <StepsComponent.Title hideBelow="sm">{stepInfo.name}</StepsComponent.Title>
                {step.isCurrent && (
                  <StepsComponent.Description hideBelow="sm">
                    {isYourTurn ? "It's your turn" : "It's your opponent's turn"}
                  </StepsComponent.Description>
                )}
              </Box>
              <StepsComponent.Separator hideBelow="sm" />
            </StepsComponent.Item>
          );
        })}
      </StepsComponent.List>
    </StepsComponent.Root>
  );
}
