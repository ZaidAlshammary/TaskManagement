export class Task {
    id: number = 0;
    title: string = '';
    description: string = '';
    dueDate: Date = new Date();
    status: Status = Status.pending;
    assignee: string = '';
    assigneeId: string = '';
}



export enum Status{
    pending,
    completed
}