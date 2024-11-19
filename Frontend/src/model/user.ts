export interface User {
  id: string;
  name: string;
  email: string;
  userType: "Customer" | "Admin";
  accountType: "Server" | "Firebase";
  createdAt?: Date;
  updatedAt?: Date;
}
