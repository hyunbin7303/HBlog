export interface Post {
    id: number;
    title: string;
    desc: string;
    status: string;
    content: string;
    type: string;
    upvotes: number;
    linkForPost: string;
    created: Date;
    LastUpdated: Date;
    userId: number;
    userName: string;
}
