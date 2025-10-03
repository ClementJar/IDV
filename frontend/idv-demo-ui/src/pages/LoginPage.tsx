import React, { useState } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { authAPI } from '../services/api';
import { LoadingState, ErrorState } from '../types';

export const LoginPage: React.FC = () => {
  const { login, isAuthenticated } = useAuth();
  const location = useLocation();
  const [formData, setFormData] = useState({
    username: 'admin',
    password: 'Admin@123',
  });
  const [loading, setLoading] = useState<LoadingState>({ isLoading: false });
  const [error, setError] = useState<ErrorState>({ hasError: false });

  // Redirect if already authenticated
  if (isAuthenticated) {
    const from = (location.state as any)?.from?.pathname || '/dashboard';
    return <Navigate to={from} replace />;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading({ isLoading: true, message: 'Signing in...' });
    setError({ hasError: false });

    try {
      const response = await authAPI.login(formData);
      login(response.token, response.user);
    } catch (err: any) {
      setError({
        hasError: true,
        message: err.response?.data?.message || 'Login failed. Please try again.',
      });
    } finally {
      setLoading({ isLoading: false });
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-red-50 via-white to-red-50">
      {/* Header */}
      <div className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-4">
            <div className="flex items-center space-x-3">
              <div className="h-10 w-10 bg-red-600 rounded-lg flex items-center justify-center">
                <span className="text-white font-bold text-sm">P</span>
              </div>
              <div>
                <h1 className="text-xl font-bold text-gray-900">Prudential</h1>
                <p className="text-xs text-gray-500">Identity Verification System</p>
              </div>
            </div>
            <div className="text-right">
              <p className="text-sm text-gray-600">Powered by</p>
              <p className="text-sm font-semibold text-red-600">Ekwantu Consulting</p>
            </div>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="flex items-center justify-center px-4 sm:px-6 lg:px-8 pt-20">
        <div className="max-w-md w-full">
          {/* Login Card */}
          <div className="bg-white rounded-xl shadow-xl border border-gray-100 overflow-hidden">
            <div className="bg-gradient-to-r from-red-600 to-red-700 px-6 py-8">
              <div className="text-center">
                <div className="mx-auto h-16 w-16 bg-white rounded-full flex items-center justify-center mb-4">
                  <svg className="h-8 w-8 text-red-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-4.618 1.04A11.955 11.955 0 012.944 12H2.944a11.955 11.955 0 004.618 9.056A11.955 11.955 0 0112 21.056a11.955 11.955 0 014.618-9.056A11.955 11.955 0 0121.056 12A11.955 11.955 0 0121.056 12a11.955 11.955 0 01-4.618-9.056z" />
                  </svg>
                </div>
                <h2 className="text-2xl font-bold text-white mb-2">
                  IDV Portal
                </h2>
                <p className="text-red-100 text-sm">
                  Secure Identity Verification System
                </p>
              </div>
            </div>
            
            <div className="px-6 py-8">
              <form onSubmit={handleSubmit} className="space-y-6">
                {error.hasError && (
                  <div className="rounded-lg bg-red-50 border border-red-200 p-4">
                    <div className="flex">
                      <div className="flex-shrink-0">
                        <svg className="h-5 w-5 text-red-400" fill="currentColor" viewBox="0 0 20 20">
                          <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                        </svg>
                      </div>
                      <div className="ml-3">
                        <p className="text-sm text-red-800">{error.message}</p>
                      </div>
                    </div>
                  </div>
                )}
                
                <div className="space-y-4">
                  <div>
                    <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-2">
                      Username
                    </label>
                    <input
                      id="username"
                      name="username"
                      type="text"
                      required
                      className="block w-full px-3 py-3 border border-gray-300 rounded-lg shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-red-500 sm:text-sm"
                      placeholder="Enter your username"
                      value={formData.username}
                      onChange={handleChange}
                      disabled={loading.isLoading}
                    />
                  </div>
                  
                  <div>
                    <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-2">
                      Password
                    </label>
                    <input
                      id="password"
                      name="password"
                      type="password"
                      required
                      className="block w-full px-3 py-3 border border-gray-300 rounded-lg shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500 focus:border-red-500 sm:text-sm"
                      placeholder="Enter your password"
                      value={formData.password}
                      onChange={handleChange}
                      disabled={loading.isLoading}
                    />
                  </div>
                </div>

                <button
                  type="submit"
                  disabled={loading.isLoading}
                  className="w-full flex justify-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-gradient-to-r from-red-600 to-red-700 hover:from-red-700 hover:to-red-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200"
                >
                  {loading.isLoading ? (
                    <>
                      <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" fill="none" viewBox="0 0 24 24">
                        <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                        <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                      </svg>
                      {loading.message}
                    </>
                  ) : (
                    <>
                      <svg className="mr-2 h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1" />
                      </svg>
                      Sign In
                    </>
                  )}
                </button>
              </form>
            </div>
            
            {/* Demo Credentials */}
            <div className="bg-gray-50 px-6 py-4 border-t">
              <div className="text-center">
                <p className="text-sm font-medium text-gray-700 mb-3">Quick Demo Login</p>
                <div className="grid grid-cols-1 gap-2">
                  <button
                    type="button"
                    onClick={() => setFormData({ username: 'admin', password: 'Admin@123' })}
                    className="flex justify-between items-center bg-white hover:bg-gray-50 rounded px-3 py-2 border border-gray-200 hover:border-red-300 transition-colors cursor-pointer text-xs"
                  >
                    <span className="font-medium text-gray-600">Admin:</span>
                    <span className="font-mono text-gray-800">admin / Admin@123</span>
                  </button>
                  <button
                    type="button"
                    onClick={() => setFormData({ username: 'agent', password: 'Agent@123' })}
                    className="flex justify-between items-center bg-white hover:bg-gray-50 rounded px-3 py-2 border border-gray-200 hover:border-red-300 transition-colors cursor-pointer text-xs"
                  >
                    <span className="font-medium text-gray-600">Agent:</span>
                    <span className="font-mono text-gray-800">agent / Agent@123</span>
                  </button>
                  <button
                    type="button"
                    onClick={() => setFormData({ username: 'viewer', password: 'Viewer@123' })}
                    className="flex justify-between items-center bg-white hover:bg-gray-50 rounded px-3 py-2 border border-gray-200 hover:border-red-300 transition-colors cursor-pointer text-xs"
                  >
                    <span className="font-medium text-gray-600">Viewer:</span>
                    <span className="font-mono text-gray-800">viewer / Viewer@123</span>
                  </button>
                </div>
                <p className="text-xs text-gray-500 mt-3">Click any credential above to auto-fill the form</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};