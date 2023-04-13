import { User } from "./user";

export class UserParams {
    gender: string;
    minAge = 18;
    maxAge = 100;
    pageNumber = 1;
    pageSize = 5;
    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
}