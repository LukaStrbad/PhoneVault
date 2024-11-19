export interface User {
  id: string;
  name: string;
  email: string;
  userType: "Customer" | "Admin";
  createdAt?: Date;
  updatedAt?: Date;
}
