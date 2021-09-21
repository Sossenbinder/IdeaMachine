export type Comment = {
	commentId: number;
	commenterId: string;
	ideaId: number;
	comment: string;
	timeStamp: Date;
}

export namespace Network {
	export namespace AddComment {
		export type Request = Comment;
	}
	
	export namespace QueryComments {
		export type Request = {
			ideaId: number;
		};

		export type Response = Array<Comment>;
	}
}