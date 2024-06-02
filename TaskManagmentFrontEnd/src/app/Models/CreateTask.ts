import { Status } from "./Task";

export class TaskPayload {
    title: string = '';
    description: string = '';
    dueDate: Date = new Date();
    status: Status = Status.pending;
    assignedTo: string = '';
}


