// API Response Types
export interface ApiResponse<T = any> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
}

// Authentication Types
export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
}

export interface User {
  userId: string;
  username: string;
  email: string;
  fullName: string;
  role: 'Admin' | 'Agent' | 'Viewer';
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
}

// ID Verification Types
export interface IDVerificationRequest {
  idNumber: string;
  idType: string;
}

export interface IDVerificationResponse {
  isVerified: boolean;
  clientData?: IDSourceClient;
  verificationId: string;
  timestamp: string;
  source: string;
}

export interface IDSourceClient {
  clientId: string;
  idType: string;
  idNumber: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  mobileNumber: string;
  province: string;
  district: string;
  postalCode: string;
  source: string;
  isVerified: boolean;
  createdAt: string;
}

// Client Registration Types
export interface ClientRegistrationRequest {
  idNumber: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  mobileNumber: string;
  email?: string;
  province: string;
  district: string;
  postalCode: string;
  productIds: string[];
}

export interface RegisteredClient {
  clientId: string;
  idNumber: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  mobileNumber: string;
  email?: string;
  province: string;
  district: string;
  postalCode: string;
  isActive: boolean;
  registrationDate: string;
  products: ClientProduct[];
}

export interface ClientProduct {
  clientProductId: string;
  clientId: string;
  productId: string;
  product: Product;
  enrollmentDate: string;
  premiumAmount: number;
  status: string;
}

// Product Types
export interface Product {
  productId: string;
  productCode: string;
  productName: string;
  category: string;
  description: string;
  premiumAmount: number;
  currency: string;
  isActive: boolean;
  createdAt: string;
}

export interface EnhancedProduct extends Product {
  premiumRange: string;
  coverageRange: string;
}

// UI State Types
export interface LoadingState {
  isLoading: boolean;
  message?: string;
}

export interface ErrorState {
  hasError: boolean;
  message?: string;
  details?: string[];
}

// Form Types
export interface FormErrors {
  [key: string]: string;
}

// Dashboard Types
export interface DashboardStats {
  totalClients: number;
  todayRegistrations: number;
  totalVerifications: number;
  totalProducts: number;
  successRate: number;
  avgResponseTime: number;
  recentActivity: ActivityLog[];
}

export interface VerificationSourceStats {
  source: string;
  count: number;
  percentage: number;
}

export interface ActivityLog {
  id: string;
  action: string;
  description: string;
  timestamp: string;
  userId: string;
  userName: string;
}

// Navigation Types
export interface NavigationItem {
  path: string;
  label: string;
  icon: string;
  roles: string[];
}

// Multi-source verification types
export interface MultiSourceVerificationResponse {
  success: boolean;
  idNumber: string;
  sourceResults: SourceSearchResult[];
  finalResult?: ClientSearchResult;
  totalResponseTime: number;
  overallStatus: string;
}

export interface SourceSearchResult {
  sourceName: string;
  displayName: string;
  status: string; // Checking, Found, NotFound, Error, Timeout, Skipped
  responseTime: number;
  isFound: boolean;
  result?: ClientSearchResult;
  errorMessage?: string;
  priority: number;
}

export interface ClientSearchResult {
  clientId: string;
  idType: string;
  idNumber: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  mobileNumber: string;
  province: string;
  district: string;
  postalCode: string;
  source: string;
  isVerified: boolean;
}

export interface AvailableTestId {
  idNumber: string;
  fullName: string;
  source: string;
  displaySource: string;
}

// EPOS Integration Types
export interface EposPayload {
  id_type: string;
  id_number: string;
  full_name: string;
  date_of_birth: string;
  gender: string;
  mobile_number: string;
  address: {
    province: string;
    district: string;
    postal_code: string;
  };
  source: string;
  captured_by: string;
  capture_timestamp: string;
  products?: EposProduct[];
}

export interface EposProduct {
  productId: string;
  productName: string;
  productCode: string;
  premiumAmount: number;
  policyNumber: string;
  status: string;
}

export interface ClientRegistrationWithEpos {
  success: boolean;
  message: string;
  registrationId: string;
  client: RegisteredClient;
  eposPayload: EposPayload;
}