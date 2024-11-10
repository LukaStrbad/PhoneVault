export interface User {
  id: number;
  name: string;
  email: string;
  userType: "Customer" | "Admin";
}
