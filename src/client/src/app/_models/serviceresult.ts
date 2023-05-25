export interface ServiceResult {
    isSuccess : boolean;
    message : string;
    errors: string[];
    value: any;
}