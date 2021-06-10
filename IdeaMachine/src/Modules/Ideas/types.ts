export type Idea = {
	id: string;
	shortDescription: string;
	longDescription: string;
	creationDate: Date;
}

export namespace Network {
	export namespace Add {
		export type Request = Idea;
	}

	export namespace Get {
		export type Response = Array<Idea>;
	}
}