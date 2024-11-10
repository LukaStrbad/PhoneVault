export interface Review {
  id: number;
  rating: number;
  comment: string;
  createdAt: Date;
  userName: string | null;
}
