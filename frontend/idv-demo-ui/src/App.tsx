import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from './contexts/AuthContext';
import { ProtectedRoute } from './components/auth/ProtectedRoute';
import { Layout } from './components/layout/Layout';
import { LoginPage } from './pages/LoginPage';
import { VerificationPage } from './pages/VerificationPage';
import { clientAPI, productAPI, verificationAPI } from './services/api';

// Create a client
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

// Enhanced Dashboard component with realistic analytics
const DashboardPage = () => {
  const [stats, setStats] = useState({
    totalClients: 0,
    todayVerifications: 0,
    successRate: 0,
    activeProducts: 23,
    avgResponseTime: 0
  });

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const clients = await clientAPI.getAll();
        const today = new Date().toDateString();
        const todayRegistrations = clients.filter(c => 
          new Date(c.registrationDate).toDateString() === today
        ).length;
        
        setStats({
          totalClients: clients.length,
          todayVerifications: Math.floor(Math.random() * 25) + 15, // Simulated daily verifications
          successRate: 94.2 + Math.random() * 4, // 94-98% success rate
          activeProducts: 23,
          avgResponseTime: 1.2 + Math.random() * 0.8 // 1.2-2.0 seconds
        });
      } catch (error) {
        console.error('Failed to fetch stats:', error);
      }
    };

    fetchStats();
    const interval = setInterval(fetchStats, 30000); // Update every 30 seconds
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="px-4 sm:px-0">
      {/* Header */}
      <div className="mb-8">
        <div className="md:flex md:items-center md:justify-between">
          <div className="flex-1 min-w-0">
            <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
            <p className="mt-2 text-gray-600">Real-time IDV system analytics and performance metrics</p>
          </div>
          <div className="mt-4 md:mt-0 md:ml-4">
            <div className="flex items-center space-x-4">
              <div className="flex items-center space-x-2 text-sm text-gray-500">
                <div className="h-2 w-2 bg-green-400 rounded-full animate-pulse"></div>
                <span>Live Data</span>
              </div>
              <div className="flex items-center space-x-2 text-sm text-gray-500">
                <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
                <span>Updated: {new Date().toLocaleTimeString()}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Key Metrics Grid */}
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4 mb-8">
        <div className="bg-white overflow-hidden shadow-sm rounded-xl border border-gray-100 hover:shadow-md transition-shadow">
          <div className="p-6">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="h-12 w-12 bg-blue-100 rounded-lg flex items-center justify-center">
                  <svg className="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                  </svg>
                </div>
              </div>
              <div className="ml-4 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">Total Registered Clients</dt>
                  <dd className="text-2xl font-bold text-gray-900">{stats.totalClients.toLocaleString()}</dd>
                  <dd className="text-xs text-green-600 flex items-center">
                    <svg className="h-3 w-3 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
                    </svg>
                    +{Math.floor(stats.totalClients * 0.15)} this month
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow-sm rounded-xl border border-gray-100 hover:shadow-md transition-shadow">
          <div className="p-6">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="h-12 w-12 bg-green-100 rounded-lg flex items-center justify-center">
                  <svg className="h-6 w-6 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                  </svg>
                </div>
              </div>
              <div className="ml-4 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">Today's Verifications</dt>
                  <dd className="text-2xl font-bold text-gray-900">{stats.todayVerifications}</dd>
                  <dd className="text-xs text-blue-600">Success Rate: {stats.successRate.toFixed(1)}%</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow-sm rounded-xl border border-gray-100 hover:shadow-md transition-shadow">
          <div className="p-6">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="h-12 w-12 bg-purple-100 rounded-lg flex items-center justify-center">
                  <svg className="h-6 w-6 text-purple-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
                  </svg>
                </div>
              </div>
              <div className="ml-4 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">Avg Response Time</dt>
                  <dd className="text-2xl font-bold text-gray-900">{stats.avgResponseTime.toFixed(1)}s</dd>
                  <dd className="text-xs text-green-600">⚡ 45% faster than industry average</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow-sm rounded-xl border border-gray-100 hover:shadow-md transition-shadow">
          <div className="p-6">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="h-12 w-12 bg-amber-100 rounded-lg flex items-center justify-center">
                  <svg className="h-6 w-6 text-amber-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
                  </svg>
                </div>
              </div>
              <div className="ml-4 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">Active Products</dt>
                  <dd className="text-2xl font-bold text-gray-900">{stats.activeProducts}</dd>
                  <dd className="text-xs text-gray-600">Across 5 categories</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* ID Verification Sources Status */}
      <div className="bg-white shadow-sm rounded-xl border border-gray-100 mb-8">
        <div className="px-6 py-4 border-b border-gray-100">
          <h3 className="text-lg font-medium text-gray-900">Data Source Status</h3>
          <p className="text-sm text-gray-500">Real-time status of ID verification sources</p>
        </div>
        <div className="p-6">
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {[
              { name: 'INRIS', status: 'online', responseTime: '0.8s', uptime: '99.9%' },
              { name: 'ZRA', status: 'online', responseTime: '1.2s', uptime: '99.7%' },
              { name: 'MNO Airtel', status: 'online', responseTime: '0.6s', uptime: '99.8%' },
              { name: 'MNO MTN', status: 'online', responseTime: '0.7s', uptime: '99.9%' },
              { name: 'Zanaco Bank', status: 'online', responseTime: '1.1s', uptime: '99.5%' },
              { name: 'RTSA', status: 'online', responseTime: '0.9s', uptime: '99.6%' }
            ].map((source, index) => (
              <div key={index} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center space-x-3">
                  <div className="h-2 w-2 bg-green-400 rounded-full"></div>
                  <span className="text-sm font-medium text-gray-900">{source.name}</span>
                </div>
                <div className="text-right">
                  <div className="text-xs text-gray-500">{source.responseTime}</div>
                  <div className="text-xs text-green-600">{source.uptime}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="bg-white shadow-sm rounded-xl border border-gray-100 mb-8">
        <div className="px-6 py-4 border-b border-gray-100">
          <h3 className="text-lg font-medium text-gray-900">Recent Activity</h3>
        </div>
        <div className="p-6">
          <div className="space-y-4">
            {[
              { action: 'Client Registration', client: 'Temba Mwanza', time: '2 minutes ago', status: 'success' },
              { action: 'ID Verification', client: 'ZM304411', time: '5 minutes ago', status: 'success' },
              { action: 'Client Registration', client: 'Bwalya Mulonda', time: '8 minutes ago', status: 'success' },
              { action: 'ID Verification', client: '19800421/43/8', time: '12 minutes ago', status: 'failed' },
              { action: 'System Backup', client: 'Automated', time: '1 hour ago', status: 'success' }
            ].map((activity, index) => (
              <div key={index} className="flex items-center space-x-4">
                <div className={`h-2 w-2 rounded-full ${
                  activity.status === 'success' ? 'bg-green-400' : 'bg-red-400'
                }`}></div>
                <div className="flex-1">
                  <p className="text-sm font-medium text-gray-900">{activity.action}</p>
                  <p className="text-xs text-gray-500">{activity.client} • {activity.time}</p>
                </div>
                <div className={`px-2 py-1 rounded-full text-xs font-medium ${
                  activity.status === 'success' 
                    ? 'bg-green-100 text-green-800' 
                    : 'bg-red-100 text-red-800'
                }`}>
                  {activity.status}
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <div className="bg-white shadow-sm rounded-xl border border-gray-100">
          <div className="px-6 py-4 border-b border-gray-100">
            <h3 className="text-lg font-medium text-gray-900">Quick Actions</h3>
          </div>
        <div className="p-6">
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
            <a
              href="/verification"
              className="group relative bg-gradient-to-r from-blue-50 to-blue-100 p-4 rounded-lg border border-blue-200 hover:from-blue-100 hover:to-blue-200 transition-colors duration-200"
            >
              <div className="flex items-center space-x-3">
                <div className="h-10 w-10 bg-blue-600 rounded-lg flex items-center justify-center">
                  <svg className="h-5 w-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                </div>
                <div>
                  <p className="text-sm font-medium text-gray-900">Verify ID</p>
                  <p className="text-xs text-gray-600">Start verification process</p>
                </div>
              </div>
            </a>
            
            <a
              href="/registration"
              className="group relative bg-gradient-to-r from-green-50 to-green-100 p-4 rounded-lg border border-green-200 hover:from-green-100 hover:to-green-200 transition-colors duration-200"
            >
              <div className="flex items-center space-x-3">
                <div className="h-10 w-10 bg-green-600 rounded-lg flex items-center justify-center">
                  <svg className="h-5 w-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
                  </svg>
                </div>
                <div>
                  <p className="text-sm font-medium text-gray-900">Register Client</p>
                  <p className="text-xs text-gray-600">Add new client</p>
                </div>
              </div>
            </a>
          </div>
        </div>
      </div>

      <div className="bg-white shadow-sm rounded-xl border border-gray-100">
        <div className="px-6 py-4 border-b border-gray-100">
          <h3 className="text-lg font-medium text-gray-900">System Information</h3>
        </div>
        <div className="p-6">
          <div className="space-y-4">
            <div className="flex justify-between items-center">
              <span className="text-sm text-gray-600">Database Status</span>
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                Connected
              </span>
            </div>
            <div className="flex justify-between items-center">
              <span className="text-sm text-gray-600">API Status</span>
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                Operational
              </span>
            </div>
            <div className="flex justify-between items-center">
              <span className="text-sm text-gray-600">Last Backup</span>
              <span className="text-sm text-gray-900">Today, 12:00 AM</span>
            </div>
            <div className="flex justify-between items-center">
              <span className="text-sm text-gray-600">Version</span>
              <span className="text-sm text-gray-900">1.0.0 Demo</span>
            </div>
          </div>
        </div>
      </div>
      </div>
    </div>
  );
};

const RegistrationPage = () => {
  const [step, setStep] = useState(1);
  const [idNumber, setIdNumber] = useState('');
  const [loading, setLoading] = useState(false); // For registration
  const [verificationLoading, setVerificationLoading] = useState(false); // For ID verification
  const [clientData, setClientData] = useState<any>(null);
  const [selectedProducts, setSelectedProducts] = useState<string[]>([]);
  const [error, setError] = useState('');
  const [verificationSources, setVerificationSources] = useState<any[]>([]);
  const [currentSearchingSource, setCurrentSearchingSource] = useState<string>('');
  const [toast, setToast] = useState<{show: boolean, message: string, type: 'success' | 'error'}>({show: false, message: '', type: 'success'});
  const [availableTestIds, setAvailableTestIds] = useState<any[]>([]);

  const showToast = (message: string, type: 'success' | 'error' = 'success') => {
    setToast({show: true, message, type});
    setTimeout(() => setToast({show: false, message: '', type: 'success'}), 4000);
  };

  // Load available test IDs on component mount
  useEffect(() => {
    const loadAvailableTestIds = async () => {
      try {
        const testIds = await verificationAPI.getAvailableTestIds();
        setAvailableTestIds(testIds);
      } catch (error) {
        console.error('Failed to load available test IDs:', error);
      }
    };
    
    loadAvailableTestIds();
  }, []);

  const refreshAvailableTestIds = async () => {
    try {
      const testIds = await verificationAPI.getAvailableTestIds();
      setAvailableTestIds(testIds);
    } catch (error) {
      console.error('Failed to refresh available test IDs:', error);
    }
  };

  const detectIdType = (value: string): 'NRC' | 'Passport' | 'Driving License' => {
    // Remove all whitespace for detection
    const cleanValue = value.replace(/\s/g, '');
    
    // NRC pattern: 8 digits + slash + 2 digits + slash + 1 digit (YYYYMMDD/DD/C)
    // Also accept just digits (will be formatted automatically)
    if (/^\d{8}\/\d{2}\/\d$/.test(cleanValue) || /^\d{8,11}$/.test(cleanValue)) {
      return 'NRC';
    }
    
    // Passport pattern: ZN followed by 7 digits (Zambian standard)
    if (/^ZN\d{7}$/i.test(cleanValue)) {
      return 'Passport';
    }
    
    // Driving License pattern: ZM followed by numbers (common Zambian format)
    if (/^ZM\d+$/i.test(cleanValue)) {
      return 'Driving License';
    }
    
    // Default to NRC if it's mostly digits (8+ digits suggests birth date format)
    if (/^\d{8,}/.test(cleanValue)) {
      return 'NRC';
    }
    
    // Default to Passport for other patterns starting with letters
    return 'Passport';
  };

  const formatIdInput = (value: string) => {
    const detectedType = detectIdType(value);
    
    if (detectedType === 'NRC') {
      // Remove all non-digits for NRC
      const digits = value.replace(/\D/g, '');
      
      // Format as YYYYMMDD/DD/C (8 digits + 2 digits + 1 digit)
      if (digits.length <= 8) {
        return digits;
      } else if (digits.length <= 10) {
        return `${digits.slice(0, 8)}/${digits.slice(8)}`;
      } else {
        return `${digits.slice(0, 8)}/${digits.slice(8, 10)}/${digits.slice(10, 11)}`;
      }
    } else if (detectedType === 'Passport') {
      // For Zambian passport: ZN + 7 digits
      const cleaned = value.toUpperCase().replace(/[^A-Z0-9]/g, '');
      if (cleaned.startsWith('ZN')) {
        return cleaned.slice(0, 9); // ZN + 7 digits = 9 characters
      } else if (/^[A-Z]/.test(cleaned)) {
        return cleaned.slice(0, 9);
      } else {
        // If user starts typing numbers, prepend ZN
        return ('ZN' + cleaned).slice(0, 9);
      }
    } else if (detectedType === 'Driving License') {
      // For Zambian driving license: ZM + numbers
      const cleaned = value.toUpperCase().replace(/[^A-Z0-9]/g, '');
      if (cleaned.startsWith('ZM')) {
        return cleaned.slice(0, 8); // ZM + 6 digits = 8 characters
      } else if (/^[A-Z]/.test(cleaned)) {
        return cleaned.slice(0, 8);
      } else {
        // If user starts typing numbers, prepend ZM
        return ('ZM' + cleaned).slice(0, 8);
      }
    }
    return value;
  };

  const handleIdChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const formatted = formatIdInput(e.target.value);
    setIdNumber(formatted);
  };

  const handleSearch = async () => {
    if (!idNumber.trim()) {
      setError('Please enter an ID number');
      return;
    }
    
    setVerificationLoading(true);
    setError('');
    setVerificationSources([]);
    
    try {
      // Call the real backend API for multi-source verification
      const result = await verificationAPI.searchClientMultiSource(idNumber);
      
      if (result.success && result.finalResult) {
        // Update sources with real results from backend
        const sourcesWithResults = result.sourceResults.map(sourceResult => ({
          name: sourceResult.sourceName,
          displayName: sourceResult.displayName,
          status: sourceResult.isFound ? 'found' : 'not-found'
        }));
        
        setVerificationSources(sourcesWithResults);
        
        // Set the found client data
        const foundClient = result.finalResult;
        setClientData({
          idNumber: foundClient.idNumber,
          fullName: foundClient.fullName,
          dateOfBirth: foundClient.dateOfBirth,
          gender: foundClient.gender,
          mobileNumber: foundClient.mobileNumber,
          email: `${foundClient.fullName.toLowerCase().replace(/\s+/g, '.')}@email.com`, // Generate email from name
          province: foundClient.province,
          district: foundClient.district,
          source: foundClient.source,
          verified: foundClient.isVerified
        });
        
        setStep(2);
        showToast(`Client found in ${foundClient.source}! You can now proceed with registration.`, 'success');
      } else {
        // No client found - show all sources as not found
        const dataSources = [
          { name: 'INRIS', displayName: 'ID Registration Information System', status: 'not-found' },
          { name: 'ZRA', displayName: 'Zambia Revenue Authority', status: 'not-found' },
          { name: 'MNO_AIRTEL', displayName: 'Airtel Network Database', status: 'not-found' },
          { name: 'MNO_MTN', displayName: 'MTN Network Database', status: 'not-found' },
          { name: 'MNO_ZAMTEL', displayName: 'Zamtel Network Database', status: 'not-found' },
          { name: 'BANK_ZANACO', displayName: 'Zanaco Banking Records', status: 'not-found' },
          { name: 'BANK_FNB', displayName: 'FNB Banking Records', status: 'not-found' },
          { name: 'BANK_STANCHART', displayName: 'Standard Chartered Records', status: 'not-found' },
          { name: 'GOVT_PAYROLL', displayName: 'Government Payroll System', status: 'not-found' },
          { name: 'NAPSA', displayName: 'National Pension Scheme Authority', status: 'not-found' },
          { name: 'RTSA', displayName: 'Road Transport & Safety Agency', status: 'not-found' }
        ];
        
        setVerificationSources(dataSources);
        showToast('Client not found in any of the searched databases. Please verify the ID number.', 'error');
      }
      
      setCurrentSearchingSource('');
    } catch (err) {
      setError('Failed to retrieve client information. Please try again.');
      showToast('Failed to retrieve client information. Please try again.', 'error');
    } finally {
      setVerificationLoading(false);
    }
  };

  const getTestDataForSource = (idNumber: string, source: string) => {
    const testDatabase: Record<string, any> = {
      '150685/10/1': { source: 'INRIS', fullName: 'John Mwanza', dateOfBirth: '1985-06-15', gender: 'Male', mobileNumber: '+260977123456', province: 'Lusaka', district: 'Lusaka' },
      '180475/08/1': { source: 'ZRA', fullName: 'Peter Phiri', dateOfBirth: '1975-04-18', gender: 'Male', mobileNumber: '+260955654321', province: 'Eastern', district: 'Chipata' },
      '250791/07/1': { source: 'MNO_AIRTEL', fullName: 'David Mulenga', dateOfBirth: '1991-07-25', gender: 'Male', mobileNumber: '+260966234567', province: 'Northern', district: 'Kasama' },
      '120693/05/1': { source: 'BANK_ZANACO', fullName: 'Grace Tembo', dateOfBirth: '1993-06-12', gender: 'Female', mobileNumber: '+260977567890', province: 'Lusaka', district: 'Chongwe' }
    };
    
    const clientData = testDatabase[idNumber];
    if (clientData && clientData.source === source) {
      return {
        idNumber,
        ...clientData,
        email: `${clientData.fullName.toLowerCase().replace(' ', '.')}@email.com`,
        address: `123 Main Street, ${clientData.district}`,
        verified: true
      };
    }
    return null;
  };

  const handleProductSelect = (productId: string) => {
    setSelectedProducts(prev => 
      prev.includes(productId) 
        ? prev.filter(id => id !== productId)
        : [...prev, productId]
    );
  };

  const handleRegister = async () => {
    setLoading(true);
    try {
      // Prepare registration data
      const registrationData = {
        idNumber: clientData.idNumber,
        fullName: clientData.fullName,
        dateOfBirth: clientData.dateOfBirth,
        gender: clientData.gender,
        mobileNumber: clientData.mobileNumber,
        email: clientData.email || '',
        province: clientData.province,
        district: clientData.district || 'Unknown',
        postalCode: '00000', // Default postal code
        productIds: selectedProducts
      };
      
      // Call real API
      const response = await clientAPI.register(registrationData);
      const registrationId = response.clientId || 'Generated Successfully';
      showToast(`Client registered successfully! Registration ID: ${registrationId}`, 'success');
      
      // Refresh available test IDs since one was just registered
      await refreshAvailableTestIds();
      
      // Reset form
      setStep(1);
      setIdNumber('');
      setClientData(null);
      setSelectedProducts([]);
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || 'Registration failed. Please try again.';
      setError(errorMessage);
      showToast(errorMessage, 'error');
    } finally {
      setLoading(false);
    }
  };

  const [products, setProducts] = useState([
    { productId: '1', productName: 'Term Life Insurance', productCode: 'LIFE001', premiumAmount: 50, category: 'Life', description: 'Basic term life insurance coverage', currency: 'ZMW', isActive: true, createdAt: '' },
    { productId: '2', productName: 'Family Health Plan', productCode: 'HEALTH002', premiumAmount: 120, category: 'Health', description: 'Comprehensive family health coverage', currency: 'ZMW', isActive: true, createdAt: '' },
    { productId: '3', productName: 'Motor Vehicle Insurance', productCode: 'MOTOR001', premiumAmount: 80, category: 'Motor', description: 'Vehicle insurance coverage', currency: 'ZMW', isActive: true, createdAt: '' },
    { productId: '4', productName: 'Flexible Investment Plan', productCode: 'INVEST002', premiumAmount: 300, category: 'Investment', description: 'Flexible investment savings plan', currency: 'ZMW', isActive: true, createdAt: '' }
  ]);

  useEffect(() => {
    // Load real products from API
    const loadProducts = async () => {
      try {
        const productData = await productAPI.getAll();
        if (productData && productData.length > 0) {
          setProducts(productData);
        }
      } catch (error) {
        console.log('Using fallback product data');
      }
    };
    loadProducts();
  }, []);

  return (
    <div className="px-4 sm:px-0">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Client Registration</h1>
        <p className="mt-2 text-gray-600">Search for clients in external systems and register them with insurance products</p>
        
        {/* Progress Indicator */}
        <div className="mt-6">
          <nav aria-label="Progress">
            <ol className="flex items-center">
              <li className="relative">
                <div className={`${step >= 1 ? 'bg-red-600' : 'bg-gray-200'} h-8 w-8 rounded-full flex items-center justify-center text-white font-medium text-sm`}>
                  1
                </div>
                <span className="ml-2 text-sm font-medium text-gray-900">ID Search</span>
              </li>
              <li className="relative ml-8">
                <div className={`${step >= 2 ? 'bg-red-600' : 'bg-gray-200'} h-8 w-8 rounded-full flex items-center justify-center text-white font-medium text-sm`}>
                  2
                </div>
                <span className="ml-2 text-sm font-medium text-gray-900">Verify & Attach Products</span>
              </li>
              <li className="relative ml-8">
                <div className={`${step >= 3 ? 'bg-red-600' : 'bg-gray-200'} h-8 w-8 rounded-full flex items-center justify-center text-white font-medium text-sm`}>
                  3
                </div>
                <span className="ml-2 text-sm font-medium text-gray-900">Complete Registration</span>
              </li>
            </ol>
          </nav>
        </div>
      </div>

      {/* Registration Flow */}
      <div className="bg-white shadow-sm rounded-xl border border-gray-100">
        {step === 1 && (
          <>
            <div className="px-6 py-4 border-b border-gray-100">
              <h2 className="text-lg font-semibold text-gray-900">Search Client by ID</h2>
              <p className="text-sm text-gray-600">Enter the client's ID document to retrieve their information from external systems</p>
            </div>
            <div className="p-6">
              {error && (
                <div className="mb-4 rounded-lg bg-red-50 border border-red-200 p-4">
                  <div className="flex">
                    <div className="flex-shrink-0">
                      <svg className="h-5 w-5 text-red-400" fill="currentColor" viewBox="0 0 20 20">
                        <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                      </svg>
                    </div>
                    <div className="ml-3">
                      <p className="text-sm text-red-800">{error}</p>
                    </div>
                  </div>
                </div>
              )}
              
              <div className="max-w-lg">
                <label className="block text-sm font-medium text-gray-700 mb-2">ID Number</label>
                <div className="flex gap-3">
                  <input
                    type="text"
                    value={idNumber}
                    onChange={handleIdChange}
                    className="flex-1 border-gray-300 rounded-lg shadow-sm focus:ring-red-500 focus:border-red-500"
                    placeholder="Enter NRC, Passport, or Driving License"
                    disabled={verificationLoading}
                  />
                  <button
                    onClick={handleSearch}
                    disabled={verificationLoading || !idNumber.trim()}
                    className="bg-red-600 text-white px-6 py-2 rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 disabled:opacity-50 disabled:cursor-not-allowed flex items-center"
                  >
                    {verificationLoading ? (
                      <>
                        <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                          <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                          <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                        </svg>
                        Searching...
                      </>
                    ) : (
                      <>
                        <svg className="mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                        </svg>
                        Search
                      </>
                    )}
                  </button>
                </div>
                {idNumber && (
                  <div className="mt-2">
                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                      Detected: {detectIdType(idNumber)} 
                      {detectIdType(idNumber) === 'NRC' ? ' 🆔' : 
                       detectIdType(idNumber) === 'Passport' ? ' 📘' : ' 🚗'}
                    </span>
                  </div>
                )}
                <div className="bg-green-50 border border-green-200 rounded-lg p-3 mt-4">
                  <p className="text-sm text-green-700">
                    <strong>Supported Formats:</strong> NRC (YYYYMMDD/DD/C), Passport (ZN1234567), Driving License (ZM123456)
                  </p>
                </div>
                <p className="mt-2 text-sm text-gray-500">
                  This will automatically detect the ID type and search the appropriate government databases
                </p>
              </div>
              

              
              {/* Available Test ID Examples */}
              <div className="mt-6 bg-blue-50 rounded-lg p-4">
                <h3 className="font-medium text-blue-900 mb-2">Available Test ID Numbers (Click to use):</h3>
                {availableTestIds.length > 0 ? (
                  <>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                      {availableTestIds.map(demo => (
                        <button
                          key={demo.idNumber}
                          onClick={() => setIdNumber(demo.idNumber)}
                          className="text-left text-sm bg-white border border-blue-200 rounded-lg px-3 py-2 hover:bg-blue-100 text-blue-700 transition-colors"
                        >
                          <div className="font-medium">{demo.idNumber}</div>
                          <div className="text-xs text-blue-500">{demo.fullName} • Found in {demo.displaySource}</div>
                        </button>
                      ))}
                    </div>
                    <p className="text-xs text-blue-600 mt-2">Only showing unregistered IDs available for testing</p>
                  </>
                ) : (
                  <div className="text-sm text-blue-600">Loading available test IDs...</div>
                )}
              </div>
            </div>
          </>
        )}

        {step === 2 && clientData && (
          <>
            <div className="px-6 py-4 border-b border-gray-100">
              <h2 className="text-lg font-semibold text-gray-900">Client Found - Attach Products</h2>
              <p className="text-sm text-gray-600">Client retrieved from {clientData.source}. Select insurance products to attach.</p>
            </div>
            <div className="p-6">
              {/* Client Information */}
              <div className="bg-green-50 border border-green-200 rounded-lg p-4 mb-6">
                <div className="flex items-start">
                  <div className="flex-shrink-0">
                    <svg className="h-5 w-5 text-green-400" fill="currentColor" viewBox="0 0 20 20">
                      <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
                    </svg>
                  </div>
                  <div className="ml-3 flex-1">
                    <h3 className="text-sm font-medium text-green-800 mb-2">✅ Client Verification Successful</h3>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm text-green-700">
                      <div><strong>Name:</strong> {clientData.fullName}</div>
                      <div><strong>ID Number:</strong> {clientData.idNumber}</div>
                      <div><strong>Date of Birth:</strong> {clientData.dateOfBirth}</div>
                      <div><strong>Gender:</strong> {clientData.gender}</div>
                      <div><strong>Mobile:</strong> {clientData.mobileNumber}</div>
                      <div><strong>Province:</strong> {clientData.province}</div>
                    </div>
                  </div>
                </div>
              </div>

              {/* Product Selection */}
              <h3 className="font-medium text-gray-900 mb-4">Select Insurance Products</h3>
              <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                {products.map((product) => (
                  <div
                    key={product.productId}
                    className={`border rounded-lg p-4 cursor-pointer transition-colors duration-200 ${
                      selectedProducts.includes(product.productId)
                        ? 'border-red-500 bg-red-50'
                        : 'border-gray-200 hover:border-gray-300'
                    }`}
                    onClick={() => handleProductSelect(product.productId)}
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <h4 className="font-medium text-gray-900">{product.productName}</h4>
                        <p className="text-sm text-gray-600">{product.productCode}</p>
                        <p className="text-sm text-gray-500 mt-1">{product.category} Insurance</p>
                      </div>
                      <div className="text-right">
                        <p className="text-lg font-bold text-red-600">{product.currency} {product.premiumAmount}</p>
                        <p className="text-xs text-gray-500">per month</p>
                      </div>
                    </div>
                    <div className="mt-3">
                      <input
                        type="checkbox"
                        checked={selectedProducts.includes(product.productId)}
                        onChange={() => handleProductSelect(product.productId)}
                        className="h-4 w-4 text-red-600 focus:ring-red-500 border-gray-300 rounded"
                      />
                      <span className="ml-2 text-sm text-gray-700">Select this product</span>
                    </div>
                  </div>
                ))}
              </div>
              
              <div className="mt-6 flex justify-between">
                <button
                  onClick={() => setStep(1)}
                  className="border border-gray-300 text-gray-700 px-6 py-2 rounded-lg hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-red-500"
                >
                  Back to Search
                </button>
                <button
                  onClick={() => setStep(3)}
                  disabled={selectedProducts.length === 0}
                  className="bg-red-600 text-white px-6 py-2 rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Review & Register
                </button>
              </div>
            </div>
          </>
        )}

        {step === 3 && clientData && (
          <>
            <div className="px-6 py-4 border-b border-gray-100">
              <h2 className="text-lg font-semibold text-gray-900">Complete Registration</h2>
              <p className="text-sm text-gray-600">Review all details and complete the client registration</p>
            </div>
            <div className="p-6">
              <div className="bg-gray-50 rounded-lg p-4 mb-6">
                <h3 className="font-medium text-gray-900 mb-3">Client Information</h3>
                <div className="grid grid-cols-2 gap-4 text-sm">
                  <div><span className="text-gray-600">Name:</span> {clientData.fullName}</div>
                  <div><span className="text-gray-600">ID:</span> {clientData.idNumber}</div>
                  <div><span className="text-gray-600">Gender:</span> {clientData.gender}</div>
                  <div><span className="text-gray-600">Mobile:</span> {clientData.mobileNumber}</div>
                  <div><span className="text-gray-600">Province:</span> {clientData.province}</div>
                  <div><span className="text-gray-600">Source:</span> {clientData.source}</div>
                </div>
              </div>
              
              <div className="bg-red-50 rounded-lg p-4">
                <h3 className="font-medium text-gray-900 mb-3">Selected Products ({selectedProducts.length})</h3>
                <div className="space-y-2">
                  {selectedProducts.map(productId => {
                    const product = products.find(p => p.productId === productId);
                    return product ? (
                      <div key={productId} className="flex justify-between items-center text-sm">
                        <span>{product.productName}</span>
                        <span className="font-medium">{product.currency} {product.premiumAmount}/month</span>
                      </div>
                    ) : null;
                  })}
                </div>
                <div className="border-t border-red-200 mt-3 pt-3">
                  <div className="flex justify-between items-center font-medium">
                    <span>Total Monthly Premium:</span>
                    <span className="text-red-600">
                      ZMW {selectedProducts.reduce((total, productId) => {
                        const product = products.find(p => p.productId === productId);
                        return total + (product?.premiumAmount || 0);
                      }, 0)}
                    </span>
                  </div>
                </div>
              </div>
              
              <div className="mt-6 flex justify-between">
                <button
                  onClick={() => setStep(2)}
                  className="border border-gray-300 text-gray-700 px-6 py-2 rounded-lg hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-red-500"
                >
                  Back to Products
                </button>
                <button 
                  onClick={handleRegister}
                  disabled={loading}
                  className="bg-green-600 text-white px-6 py-2 rounded-lg hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 disabled:opacity-50 flex items-center"
                >
                  {loading ? (
                    <>
                      <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                        <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                        <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                      </svg>
                      Registering...
                    </>
                  ) : (
                    '✅ Complete Registration'
                  )}
                </button>
              </div>
            </div>
          </>
        )}
      </div>

      {/* Professional Loading Overlay */}
      {verificationLoading && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm">
          <div className="bg-white rounded-2xl p-8 max-w-sm w-full mx-4 shadow-2xl">
            <div className="text-center">
              {/* Status Text with Integrated Spinner */}
              <div className="flex items-center justify-center mb-4">
                <div className="w-6 h-6 mr-3">
                  <div className="w-full h-full border-2 border-gray-200 rounded-full"></div>
                  <div className="absolute w-6 h-6 border-2 border-red-600 rounded-full animate-spin border-t-transparent -mt-6"></div>
                </div>
                <h3 className="text-xl font-semibold text-gray-900">Verifying Identity</h3>
              </div>
              <p className="text-gray-600">
                {currentSearchingSource || 'Connecting to verification systems...'}
              </p>
            </div>
          </div>
        </div>
      )}

      {/* Toast Notification */}
      {toast.show && (
        <div className="fixed top-4 right-4 z-50 animate-fade-in-down">
          <div className={`rounded-lg px-6 py-4 shadow-lg max-w-sm ${
            toast.type === 'success' 
              ? 'bg-green-600 text-white' 
              : 'bg-red-600 text-white'
          }`}>
            <div className="flex items-center">
              {toast.type === 'success' ? (
                <svg className="w-5 h-5 mr-3 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                </svg>
              ) : (
                <svg className="w-5 h-5 mr-3 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                </svg>
              )}
              <p className="text-sm font-medium">{toast.message}</p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

// Export functions
const exportToCSV = (data: any[], filename: string) => {
  if (data.length === 0) return;
  
  const headers = ['ID Number', 'Full Name', 'Gender', 'Mobile Number', 'Email', 'Province', 'District', 'Registration Date', 'Status'];
  const csvContent = [
    headers.join(','),
    ...data.map(client => [
      client.idNumber || '',
      `"${client.fullName || ''}"`,
      client.gender || '',
      client.mobileNumber || '',
      client.email || '',
      client.province || '',
      client.district || '',
      client.registrationDate ? new Date(client.registrationDate).toLocaleDateString() : '',
      client.status || 'Active'
    ].join(','))
  ].join('\n');
  
  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement('a');
  link.href = URL.createObjectURL(blob);
  link.download = `${filename}-${new Date().toISOString().split('T')[0]}.csv`;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

const exportToPDF = (data: any[], filename: string) => {
  // Simple PDF-like HTML export (opens in new window for printing)
  const htmlContent = `
    <!DOCTYPE html>
    <html>
    <head>
      <title>Registered Clients Report</title>
      <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #1f2937; border-bottom: 2px solid #e5e7eb; padding-bottom: 10px; }
        table { width: 100%; border-collapse: collapse; margin-top: 20px; }
        th, td { border: 1px solid #d1d5db; padding: 8px; text-align: left; }
        th { background-color: #f3f4f6; font-weight: bold; }
        tr:nth-child(even) { background-color: #f9fafb; }
        .report-meta { color: #6b7280; font-size: 14px; margin-bottom: 20px; }
      </style>
    </head>
    <body>
      <h1>Registered Clients Report</h1>
      <div class="report-meta">
        Generated on: ${new Date().toLocaleDateString()} | Total Records: ${data.length}
      </div>
      <table>
        <thead>
          <tr>
            <th>ID Number</th>
            <th>Full Name</th>
            <th>Gender</th>
            <th>Mobile</th>
            <th>Province</th>
            <th>Registration Date</th>
          </tr>
        </thead>
        <tbody>
          ${data.map(client => `
            <tr>
              <td>${client.idNumber || ''}</td>
              <td>${client.fullName || ''}</td>
              <td>${client.gender || ''}</td>
              <td>${client.mobileNumber || ''}</td>
              <td>${client.province || ''}</td>
              <td>${client.registrationDate ? new Date(client.registrationDate).toLocaleDateString() : ''}</td>
            </tr>
          `).join('')}
        </tbody>
      </table>
    </body>
    </html>
  `;
  
  const printWindow = window.open('', '_blank');
  if (printWindow) {
    printWindow.document.write(htmlContent);
    printWindow.document.close();
    printWindow.focus();
    setTimeout(() => printWindow.print(), 500);
  }
};

const ClientsPage = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [clients, setClients] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchClients();
  }, []);

  const fetchClients = async () => {
    try {
      setLoading(true);
      const response = await clientAPI.getAll();
      setClients(response);
    } catch (err: any) {
      setError('Failed to load clients');
      console.error('Error fetching clients:', err);
    } finally {
      setLoading(false);
    }
  };

  const filteredClients = clients.filter(client =>
    client.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    client.idNumber.includes(searchTerm)
  );

  return (
    <div className="px-4 sm:px-0">
      {/* Header */}
      <div className="mb-8">
        <div className="md:flex md:items-center md:justify-between">
          <div className="flex-1 min-w-0">
            <h1 className="text-3xl font-bold text-gray-900">Registered Clients</h1>
            <p className="mt-2 text-gray-600">Manage and view all registered clients</p>
          </div>
          <div className="mt-4 md:mt-0">
            <div className="flex items-center space-x-4">
              <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-green-100 text-green-800">
                {clients.length} Active Clients
              </span>
              <button 
                onClick={() => exportToCSV(filteredClients, 'registered-clients')}
                className="bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 flex items-center space-x-2"
              >
                <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
                <span>Export CSV</span>
              </button>
              <button 
                onClick={() => exportToPDF(filteredClients, 'registered-clients')}
                className="bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 flex items-center space-x-2"
              >
                <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
                </svg>
                <span>Export PDF</span>
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Search and Filters */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 mb-6">
        <div className="p-6">
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between space-y-4 sm:space-y-0 sm:space-x-4">
            <div className="flex-1">
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <svg className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                </div>
                <input
                  type="text"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500"
                  placeholder="Search by name or ID number..."
                />
              </div>
            </div>
            <div className="flex items-center space-x-2">
              <select className="border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500">
                <option>All Provinces</option>
                <option>Lusaka</option>
                <option>Copperbelt</option>
                <option>Western</option>
              </select>
              <select className="border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500">
                <option>All Status</option>
                <option>Active</option>
                <option>Inactive</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      {/* Clients Table */}
      <div className="bg-white shadow-sm rounded-xl border border-gray-100">
        <div className="px-6 py-4 border-b border-gray-100">
          <h3 className="text-lg font-semibold text-gray-900">Client List</h3>
        </div>
        {loading ? (
          <div className="flex items-center justify-center py-12">
            <div className="flex items-center space-x-2">
              <svg className="animate-spin h-5 w-5 text-red-600" fill="none" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              <span className="text-gray-600">Loading clients...</span>
            </div>
          </div>
        ) : error ? (
          <div className="p-6 text-center">
            <div className="text-red-600 mb-2">Failed to load clients</div>
            <button 
              onClick={fetchClients}
              className="text-red-600 hover:text-red-800 underline"
            >
              Try again
            </button>
          </div>
        ) : filteredClients.length === 0 ? (
          <div className="p-6 text-center text-gray-500">
            {searchTerm ? 'No clients found matching your search.' : 'No clients registered yet.'}
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Client Details
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Contact Info
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Products
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {filteredClients.map((client) => (
                  <tr key={client.clientId} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div>
                        <div className="text-sm font-medium text-gray-900">{client.fullName}</div>
                        <div className="text-sm text-gray-500">ID: {client.idNumber}</div>
                        <div className="text-sm text-gray-500">{client.gender} • {client.province}</div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900">{client.mobileNumber}</div>
                      <div className="text-sm text-gray-500">
                        Registered: {new Date(client.registrationDate).toLocaleDateString()}
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="space-y-1">
                        {client.selectedProducts && client.selectedProducts.length > 0 ? (
                          client.selectedProducts.map((product: string, index: number) => (
                            <span
                              key={index}
                              className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800 mr-1"
                            >
                              {product}
                            </span>
                          ))
                        ) : (
                          <span className="text-sm text-gray-400">No products</span>
                        )}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        client.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                      }`}>
                        {client.status || 'Active'}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                      <div className="flex items-center space-x-2">
                        <button className="text-blue-600 hover:text-blue-900">View</button>
                        <button className="text-green-600 hover:text-green-900">Edit</button>
                        <button className="text-red-600 hover:text-red-900">Archive</button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
        
        {filteredClients.length === 0 && (
          <div className="text-center py-12">
            <svg className="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
            <h3 className="mt-2 text-sm font-medium text-gray-900">No clients found</h3>
            <p className="mt-1 text-sm text-gray-500">
              {searchTerm ? 'Try adjusting your search criteria.' : 'Get started by registering a new client.'}
            </p>
          </div>
        )}
      </div>
    </div>
  );
};

const ProductsPage = () => (
  <div className="px-4 sm:px-0">
    {/* Header */}
    <div className="mb-8">
      <div className="md:flex md:items-center md:justify-between">
        <div className="flex-1 min-w-0">
          <h1 className="text-3xl font-bold text-gray-900">Insurance Products</h1>
          <p className="mt-2 text-gray-600">Browse and manage available insurance products across multiple categories</p>
        </div>
        <div className="mt-4 md:mt-0">
          <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-blue-100 text-blue-800">
            23 Active Products
          </span>
        </div>
      </div>
    </div>

    {/* Product Categories */}
    <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 mb-8">
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
        <div className="flex items-center space-x-3 mb-4">
          <div className="h-10 w-10 bg-blue-100 rounded-lg flex items-center justify-center">
            <svg className="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
            </svg>
          </div>
          <div>
            <h3 className="text-lg font-semibold text-gray-900">Life Insurance</h3>
            <p className="text-sm text-gray-600">6 products</p>
          </div>
        </div>
        <p className="text-sm text-gray-600 mb-4">Comprehensive life coverage options including term life, whole life, and endowment policies.</p>
        <div className="space-y-2">
          <div className="flex justify-between text-xs">
            <span className="text-gray-600">Premium Range:</span>
            <span className="font-medium">ZMW 50 - 500</span>
          </div>
          <div className="flex justify-between text-xs">
            <span className="text-gray-600">Most Popular:</span>
            <span className="font-medium">Term Life Insurance</span>
          </div>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
        <div className="flex items-center space-x-3 mb-4">
          <div className="h-10 w-10 bg-green-100 rounded-lg flex items-center justify-center">
            <svg className="h-6 w-6 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-4.618 1.04A11.955 11.955 0 012.944 12H2.944a11.955 11.955 0 004.618 9.056A11.955 11.955 0 0112 21.056a11.955 11.955 0 014.618-9.056A11.955 11.955 0 0121.056 12A11.955 11.955 0 0121.056 12a11.955 11.955 0 01-4.618-9.056z" />
            </svg>
          </div>
          <div>
            <h3 className="text-lg font-semibold text-gray-900">Health Insurance</h3>
            <p className="text-sm text-gray-600">5 products</p>
          </div>
        </div>
        <p className="text-sm text-gray-600 mb-4">Medical coverage including individual health, family health, and critical illness protection.</p>
        <div className="space-y-2">
          <div className="flex justify-between text-xs">
            <span className="text-gray-600">Premium Range:</span>
            <span className="font-medium">ZMW 80 - 400</span>
          </div>
          <div className="flex justify-between text-xs">
            <span className="text-gray-600">Most Popular:</span>
            <span className="font-medium">Family Health Plan</span>
          </div>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
        <div className="flex items-center space-x-3 mb-4">
          <div className="h-10 w-10 bg-yellow-100 rounded-lg flex items-center justify-center">
            <svg className="h-6 w-6 text-yellow-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 100 4m0-4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 100 4m0-4v2m0-6V4" />
            </svg>
          </div>
          <div>
            <h3 className="text-lg font-semibold text-gray-900">Investment Plans</h3>
            <p className="text-sm text-gray-600">4 products</p>
          </div>
        </div>
        <p className="text-sm text-gray-600 mb-4">Investment-linked policies and savings plans for long-term wealth building.</p>
        <div className="space-y-2">
          <div className="flex justify-between text-xs">
            <span className="text-gray-600">Premium Range:</span>
            <span className="font-medium">ZMW 200 - 1000</span>
          </div>
          <div className="flex justify-between text-xs">
            <span className="text-gray-600">Most Popular:</span>
            <span className="font-medium">Flexible Investment Plan</span>
          </div>
        </div>
      </div>
    </div>

    {/* Featured Products */}
    <div className="bg-white rounded-xl shadow-sm border border-gray-100">
      <div className="px-6 py-4 border-b border-gray-100">
        <h3 className="text-lg font-semibold text-gray-900">Featured Products</h3>
        <p className="text-sm text-gray-600">Most popular insurance products</p>
      </div>
      <div className="p-6">
        <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
          <div className="border border-gray-200 rounded-lg p-4 hover:border-blue-300 transition-colors duration-200">
            <div className="flex items-start justify-between mb-3">
              <div>
                <h4 className="font-semibold text-gray-900">Term Life Insurance</h4>
                <p className="text-sm text-gray-600">LIFE001</p>
              </div>
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                Active
              </span>
            </div>
            <p className="text-sm text-gray-600 mb-3">Affordable life insurance coverage for a specified term period.</p>
            <div className="flex items-center justify-between">
              <span className="text-lg font-bold text-blue-600">ZMW 50/month</span>
              <span className="text-xs text-gray-500">Life Insurance</span>
            </div>
          </div>

          <div className="border border-gray-200 rounded-lg p-4 hover:border-blue-300 transition-colors duration-200">
            <div className="flex items-start justify-between mb-3">
              <div>
                <h4 className="font-semibold text-gray-900">Family Health Plan</h4>
                <p className="text-sm text-gray-600">HEALTH002</p>
              </div>
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                Active
              </span>
            </div>
            <p className="text-sm text-gray-600 mb-3">Comprehensive health coverage for the entire family.</p>
            <div className="flex items-center justify-between">
              <span className="text-lg font-bold text-blue-600">ZMW 120/month</span>
              <span className="text-xs text-gray-500">Health Insurance</span>
            </div>
          </div>

          <div className="border border-gray-200 rounded-lg p-4 hover:border-blue-300 transition-colors duration-200">
            <div className="flex items-start justify-between mb-3">
              <div>
                <h4 className="font-semibold text-gray-900">Motor Vehicle Insurance</h4>
                <p className="text-sm text-gray-600">MOTOR001</p>
              </div>
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                Active
              </span>
            </div>
            <p className="text-sm text-gray-600 mb-3">Comprehensive motor vehicle coverage including third party liability.</p>
            <div className="flex items-center justify-between">
              <span className="text-lg font-bold text-blue-600">ZMW 80/month</span>
              <span className="text-xs text-gray-500">Motor Insurance</span>
            </div>
          </div>

          <div className="border border-gray-200 rounded-lg p-4 hover:border-blue-300 transition-colors duration-200">
            <div className="flex items-start justify-between mb-3">
              <div>
                <h4 className="font-semibold text-gray-900">Flexible Investment Plan</h4>
                <p className="text-sm text-gray-600">INVEST002</p>
              </div>
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                Active
              </span>
            </div>
            <p className="text-sm text-gray-600 mb-3">Investment-linked plan with flexible premium payments.</p>
            <div className="flex items-center justify-between">
              <span className="text-lg font-bold text-blue-600">ZMW 300/month</span>
              <span className="text-xs text-gray-500">Investment Plan</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
);

const MIReportsPage = () => {
  const [reportData, setReportData] = useState<any>(null);
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState({
    startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString().split('T')[0],
    endDate: new Date().toISOString().split('T')[0]
  });

  const generateReport = async () => {
    setLoading(true);
    try {
      // Simulate API call for report data
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      // Mock report data
      const mockData = {
        summary: {
          totalClients: 156,
          newRegistrations: 23,
          activeProducts: 347,
          totalPremium: 45690,
          verificationRequests: 89,
          successfulVerifications: 82
        },
        clientsByProvince: [
          { province: 'Lusaka', count: 64, percentage: 41.0 },
          { province: 'Copperbelt', count: 38, percentage: 24.4 },
          { province: 'Western', count: 21, percentage: 13.5 },
          { province: 'Eastern', count: 18, percentage: 11.5 },
          { province: 'Northern', count: 15, percentage: 9.6 }
        ],
        productPerformance: [
          { name: 'Term Life Insurance', sales: 89, premium: 15640, growth: 12.5 },
          { name: 'Family Health Plan', sales: 67, premium: 18930, growth: 8.3 },
          { name: 'Motor Vehicle Insurance', sales: 52, premium: 7850, growth: 15.2 },
          { name: 'Flexible Investment Plan', sales: 34, premium: 14280, growth: 22.1 }
        ],
        verificationStats: {
          totalRequests: 89,
          successful: 82,
          failed: 7,
          avgResponseTime: 1247,
          topSources: [
            { source: 'National Registration Office', requests: 67, successRate: 94.0 },
            { source: 'PACRA Database', requests: 22, successRate: 86.4 }
          ]
        }
      };
      
      setReportData(mockData);
    } catch (error) {
      console.error('Error generating report:', error);
    } finally {
      setLoading(false);
    }
  };

  const exportToCSV = () => {
    if (!reportData) return;
    
    const csvData = [
      ['Metric', 'Value'],
      ['Total Clients', reportData.summary.totalClients],
      ['New Registrations', reportData.summary.newRegistrations],
      ['Active Products', reportData.summary.activeProducts],
      ['Total Premium (ZMW)', reportData.summary.totalPremium],
      ['Verification Requests', reportData.summary.verificationRequests],
      ['Successful Verifications', reportData.summary.successfulVerifications],
      [''],
      ['Province Distribution', ''],
      ...reportData.clientsByProvince.map((item: any) => [item.province, `${item.count} (${item.percentage}%)`]),
      [''],
      ['Product Performance', ''],
      ...reportData.productPerformance.map((item: any) => [item.name, `${item.sales} sales, ZMW ${item.premium}, ${item.growth}% growth`])
    ];

    const csvContent = csvData.map(row => row.join(',')).join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `IDV_MI_Report_${dateRange.startDate}_to_${dateRange.endDate}.csv`;
    a.click();
    window.URL.revokeObjectURL(url);
  };

  const exportToPDF = () => {
    if (!reportData) return;
    
    // In a real application, you would use a library like jsPDF or similar
    alert('PDF export would be implemented with a library like jsPDF or by calling a backend service.');
  };

  return (
    <div className="px-4 sm:px-0">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Management Information Reports</h1>
        <p className="mt-2 text-gray-600">Generate comprehensive reports on IDV system performance and client analytics</p>
      </div>

      {/* Report Controls */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6 mb-6">
        <h3 className="text-lg font-semibold text-gray-900 mb-4">Report Parameters</h3>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">Start Date</label>
            <input
              type="date"
              value={dateRange.startDate}
              onChange={(e) => setDateRange({...dateRange, startDate: e.target.value})}
              className="block w-full border-gray-300 rounded-lg shadow-sm focus:ring-red-500 focus:border-red-500"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">End Date</label>
            <input
              type="date"
              value={dateRange.endDate}
              onChange={(e) => setDateRange({...dateRange, endDate: e.target.value})}
              className="block w-full border-gray-300 rounded-lg shadow-sm focus:ring-red-500 focus:border-red-500"
            />
          </div>
          <div className="flex items-end">
            <button
              onClick={generateReport}
              disabled={loading}
              className="w-full bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 disabled:opacity-50 flex items-center justify-center"
            >
              {loading ? (
                <>
                  <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Generating...
                </>
              ) : (
                <>
                  <svg className="mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                  </svg>
                  Generate Report
                </>
              )}
            </button>
          </div>
        </div>
      </div>

      {/* Report Results */}
      {reportData && (
        <>
          {/* Export Actions */}
          <div className="flex justify-end space-x-3 mb-6">
            <button
              onClick={exportToCSV}
              className="bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 flex items-center"
            >
              <svg className="mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              Export CSV
            </button>
            <button
              onClick={exportToPDF}
              className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 flex items-center"
            >
              <svg className="mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              Export PDF
            </button>
          </div>

          {/* Summary Cards */}
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="h-8 w-8 bg-blue-100 rounded-lg flex items-center justify-center">
                    <svg className="h-5 w-5 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                  </div>
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-500">Total Clients</p>
                  <p className="text-2xl font-bold text-gray-900">{reportData.summary.totalClients}</p>
                </div>
              </div>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="h-8 w-8 bg-green-100 rounded-lg flex items-center justify-center">
                    <svg className="h-5 w-5 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
                    </svg>
                  </div>
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-500">New Registrations</p>
                  <p className="text-2xl font-bold text-gray-900">{reportData.summary.newRegistrations}</p>
                </div>
              </div>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="h-8 w-8 bg-yellow-100 rounded-lg flex items-center justify-center">
                    <svg className="h-5 w-5 text-yellow-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                  </div>
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-500">Total Premium</p>
                  <p className="text-2xl font-bold text-gray-900">ZMW {reportData.summary.totalPremium.toLocaleString()}</p>
                </div>
              </div>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="h-8 w-8 bg-purple-100 rounded-lg flex items-center justify-center">
                    <svg className="h-5 w-5 text-purple-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                  </div>
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-500">Verification Success Rate</p>
                  <p className="text-2xl font-bold text-gray-900">
                    {Math.round((reportData.summary.successfulVerifications / reportData.summary.verificationRequests) * 100)}%
                  </p>
                </div>
              </div>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="h-8 w-8 bg-red-100 rounded-lg flex items-center justify-center">
                    <svg className="h-5 w-5 text-red-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
                    </svg>
                  </div>
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-500">Active Products</p>
                  <p className="text-2xl font-bold text-gray-900">{reportData.summary.activeProducts}</p>
                </div>
              </div>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="h-8 w-8 bg-indigo-100 rounded-lg flex items-center justify-center">
                    <svg className="h-5 w-5 text-indigo-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
                    </svg>
                  </div>
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-500">Avg Response Time</p>
                  <p className="text-2xl font-bold text-gray-900">{reportData.verificationStats.avgResponseTime}ms</p>
                </div>
              </div>
            </div>
          </div>

          {/* Detailed Tables */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* Province Distribution */}
            <div className="bg-white rounded-xl shadow-sm border border-gray-100">
              <div className="px-6 py-4 border-b border-gray-100">
                <h3 className="text-lg font-semibold text-gray-900">Clients by Province</h3>
              </div>
              <div className="p-6">
                <div className="space-y-4">
                  {reportData.clientsByProvince.map((item: any, index: number) => (
                    <div key={index} className="flex items-center justify-between">
                      <div className="flex items-center">
                        <div className="w-3 h-3 rounded-full bg-red-600 mr-3"></div>
                        <span className="text-sm font-medium text-gray-900">{item.province}</span>
                      </div>
                      <div className="text-right">
                        <div className="text-sm font-bold text-gray-900">{item.count}</div>
                        <div className="text-xs text-gray-500">{item.percentage}%</div>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>

            {/* Product Performance */}
            <div className="bg-white rounded-xl shadow-sm border border-gray-100">
              <div className="px-6 py-4 border-b border-gray-100">
                <h3 className="text-lg font-semibold text-gray-900">Product Performance</h3>
              </div>
              <div className="overflow-x-auto">
                <table className="min-w-full">
                  <thead>
                    <tr className="border-b border-gray-100">
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Product</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Sales</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Premium</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Growth</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-100">
                    {reportData.productPerformance.map((item: any, index: number) => (
                      <tr key={index}>
                        <td className="px-6 py-4 text-sm font-medium text-gray-900">{item.name}</td>
                        <td className="px-6 py-4 text-sm text-gray-600">{item.sales}</td>
                        <td className="px-6 py-4 text-sm text-gray-600">ZMW {item.premium.toLocaleString()}</td>
                        <td className="px-6 py-4">
                          <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                            item.growth > 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                          }`}>
                            {item.growth > 0 ? '+' : ''}{item.growth}%
                          </span>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <Layout>
                    <DashboardPage />
                  </Layout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/verification"
              element={
                <ProtectedRoute roles={['Admin', 'Agent']}>
                  <Layout>
                    <VerificationPage />
                  </Layout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/registration"
              element={
                <ProtectedRoute roles={['Admin', 'Agent']}>
                  <Layout>
                    <RegistrationPage />
                  </Layout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/clients"
              element={
                <ProtectedRoute>
                  <Layout>
                    <ClientsPage />
                  </Layout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/products"
              element={
                <ProtectedRoute>
                  <Layout>
                    <ProductsPage />
                  </Layout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/reports"
              element={
                <ProtectedRoute roles={['Admin']}>
                  <Layout>
                    <MIReportsPage />
                  </Layout>
                </ProtectedRoute>
              }
            />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </Router>
      </AuthProvider>
    </QueryClientProvider>
  );
}

export default App;
