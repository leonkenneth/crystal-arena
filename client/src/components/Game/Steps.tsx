import { StepState } from "@/types";
import { Steps as StepsComponent, Box } from "@chakra-ui/react";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";

const nonCombatStepsToDisplay = ["Draw", "1st main", "Beg. of combat", "2nd main", "End of turn"];
const combatSteps = ["Dec. attackers", "Dec. blockers", "Combat damage"];
const stepsToDisplayInCombat = ["Dec. attackers", "Dec. blockers", "Combat damage"];

const stepsInfo: Record<string, { name: string; indicator: string }> = {
  Draw: {
    name: "Draw",
    indicator: "1",
  },
  "1st main": {
    name: "1st main",
    indicator: "2",
  },
  "Beg. of combat": {
    name: "Combat",
    indicator: "3",
  },
  "Dec. attackers": {
    name: "Dec. attackers",
    indicator: "3A",
  },
  "Dec. blockers": {
    name: "Dec. blockers",
    indicator: "3B",
  },
  "Combat damage": {
    name: "Combat damage",
    indicator: "3C",
  },
  "2nd main": {
    name: "2nd main",
    indicator: "4",
  },
  "End of turn": {
    name: "End of turn",
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
      <StepsComponent.List>
        {stepsToDisplay.map((step, index) => {
          const stepInfo = stepsInfo[step.name];
          return (
            <StepsComponent.Item key={step.oid.oid} index={index}>
              <StepsComponent.Indicator
                borderColor="fg.muted"
                border={activeStepIndex === index ? "1px solid" : "none"}
              >
                {stepInfo.indicator}
              </StepsComponent.Indicator>
              <Box>
                <StepsComponent.Title>{stepInfo.name}</StepsComponent.Title>
                {step.isCurrent && (
                  <StepsComponent.Description hideBelow="sm">
                    {isYourTurn ? "It's your turn" : "It's your opponent's turn"}
                  </StepsComponent.Description>
                )}
              </Box>
              <StepsComponent.Separator />
            </StepsComponent.Item>
          );
        })}
      </StepsComponent.List>
    </StepsComponent.Root>
  );
}
