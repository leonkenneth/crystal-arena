import { ObjectIdContainer } from "./oid";

export type StepState = {
    name: string;
    isCurrent: boolean;
    autoPass: string;
    oid: ObjectIdContainer;
}

export type StepsState = { steps: StepState[] }