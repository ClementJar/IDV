import axios, { AxiosResponse } from 'axios';
import { 
  LoginRequest, 
  LoginResponse, 
  User, 
  IDVerificationRequest,
  IDVerificationResponse,
  MultiSourceVerificationResponse,
  AvailableTestId,
  ClientRegistrationRequest,
  RegisteredClient,
  ClientRegistrationWithEpos,
  Product,
  DashboardStats
} from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5161/api';

// Create axios instance with default config
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      localStorage.removeItem('currentUser');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  login: async (credentials: LoginRequest): Promise<LoginResponse> => {
    const response: AxiosResponse<LoginResponse> = await apiClient.post('/auth/login', credentials);
    return response.data;
  },

  logout: () => {
    localStorage.removeItem('authToken');
    localStorage.removeItem('currentUser');
  },

  getCurrentUser: () => {
    const userStr = localStorage.getItem('currentUser');
    return userStr ? JSON.parse(userStr) : null;
  },

  isAuthenticated: () => {
    return !!localStorage.getItem('authToken');
  }
};

// Verification API
export const verificationAPI = {
  verifyID: async (request: IDVerificationRequest): Promise<IDVerificationResponse> => {
    const response: AxiosResponse<IDVerificationResponse> = await apiClient.post('/verification/verify', request);
    return response.data;
  },

  searchClient: async (idNumber: string): Promise<IDVerificationResponse> => {
    const encodedIdNumber = encodeURIComponent(idNumber);
    const response: AxiosResponse<IDVerificationResponse> = await apiClient.get(`/verification/search/${encodedIdNumber}`);
    return response.data;
  },

  searchClientMultiSource: async (idNumber: string): Promise<MultiSourceVerificationResponse> => {
    const encodedIdNumber = encodeURIComponent(idNumber);
    const response: AxiosResponse<MultiSourceVerificationResponse> = await apiClient.get(`/verification/multi-source/${encodedIdNumber}`);
    return response.data;
  },

  getAvailableTestIds: async (): Promise<AvailableTestId[]> => {
    const response: AxiosResponse<AvailableTestId[]> = await apiClient.get('/verification/available-test-ids');
    return response.data;
  }
};

// Client API
export const clientAPI = {
  register: async (request: ClientRegistrationRequest): Promise<ClientRegistrationWithEpos> => {
    const response: AxiosResponse<ClientRegistrationWithEpos> = await apiClient.post('/clients/register', request);
    return response.data;
  },

  getAll: async (): Promise<RegisteredClient[]> => {
    const response: AxiosResponse<RegisteredClient[]> = await apiClient.get('/clients');
    return response.data;
  },

  getById: async (id: string): Promise<RegisteredClient> => {
    const response: AxiosResponse<RegisteredClient> = await apiClient.get(`/clients/${id}`);
    return response.data;
  },

  update: async (id: string, client: Partial<RegisteredClient>): Promise<RegisteredClient> => {
    const response: AxiosResponse<RegisteredClient> = await apiClient.put(`/clients/${id}`, client);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/clients/${id}`);
  }
};

// Product API
export const productAPI = {
  getAll: async (): Promise<Product[]> => {
    const response: AxiosResponse<Product[]> = await apiClient.get('/products');
    return response.data;
  },

  getById: async (id: string): Promise<Product> => {
    const response: AxiosResponse<Product> = await apiClient.get(`/products/${id}`);
    return response.data;
  },

  getByCategory: async (category: string): Promise<Product[]> => {
    const response: AxiosResponse<Product[]> = await apiClient.get(`/products/category/${category}`);
    return response.data;
  }
};

// Dashboard API
export const dashboardAPI = {
  getStats: async (): Promise<DashboardStats> => {
    const response: AxiosResponse<DashboardStats> = await apiClient.get('/dashboard/stats');
    return response.data;
  }
};

// Reports API
export const reportsAPI = {
  generateClientReport: async (): Promise<any> => {
    const response: AxiosResponse<any> = await apiClient.get('/reports/clients');
    return response.data;
  },
  
  getDashboardStatistics: async (): Promise<any> => {
    const response: AxiosResponse<any> = await apiClient.get('/reports/dashboard-statistics');
    return response.data;
  },
  
  exportToExcel: async (): Promise<any> => {
    const response: AxiosResponse<any> = await apiClient.get('/reports/export/excel');
    return response.data;
  },
  
  exportToPdf: async (): Promise<any> => {
    const response: AxiosResponse<any> = await apiClient.get('/reports/export/pdf');
    return response.data;
  }
};

export { apiClient };