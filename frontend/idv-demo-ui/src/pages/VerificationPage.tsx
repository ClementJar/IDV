import React, { useState } from 'react';
import { verificationAPI } from '../services/api';
import { IDVerificationResponse, LoadingState, ErrorState } from '../types';

export const VerificationPage: React.FC = () => {
  const [idNumber, setIdNumber] = useState('');
  const [idType, setIdType] = useState('NationalID');
  const [verificationResult, setVerificationResult] = useState<IDVerificationResponse | null>(null);
  const [loading, setLoading] = useState<LoadingState>({ isLoading: false });
  const [error, setError] = useState<ErrorState>({ hasError: false });

  const handleVerification = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading({ isLoading: true, message: 'Verifying ID...' });
    setError({ hasError: false });
    setVerificationResult(null);

    try {
      const result = await verificationAPI.verifyID({ idNumber, idType });
      setVerificationResult(result);
    } catch (err: any) {
      setError({
        hasError: true,
        message: err.response?.data?.message || 'Verification failed. Please try again.',
      });
    } finally {
      setLoading({ isLoading: false });
    }
  };

  const handleSearch = async () => {
    if (!idNumber.trim()) return;
    
    setLoading({ isLoading: true, message: 'Searching client...' });
    setError({ hasError: false });
    setVerificationResult(null);

    try {
      const result = await verificationAPI.searchClient(idNumber);
      setVerificationResult(result);
    } catch (err: any) {
      setError({
        hasError: true,
        message: err.response?.data?.message || 'Search failed. Please try again.',
      });
    } finally {
      setLoading({ isLoading: false });
    }
  };

  return (
    <div className="px-4 sm:px-0">
      {/* Header */}
      <div className="mb-8">
        <div className="md:flex md:items-center md:justify-between">
          <div className="flex-1 min-w-0">
            <h1 className="text-3xl font-bold text-gray-900">ID Verification</h1>
            <p className="mt-2 text-gray-600">
              Verify client identity using National ID or other identification documents from external sources.
            </p>
          </div>
          <div className="mt-4 md:mt-0">
            <div className="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800">
              <div className="w-2 h-2 bg-green-400 rounded-full mr-2"></div>
              System Online
            </div>
          </div>
        </div>
      </div>

      {/* Verification Form */}
      <div className="bg-white shadow-sm rounded-xl border border-gray-100 mb-8">
        <div className="px-6 py-4 border-b border-gray-100 bg-gradient-to-r from-blue-50 to-blue-100">
          <div className="flex items-center space-x-3">
            <div className="h-8 w-8 bg-blue-600 rounded-lg flex items-center justify-center">
              <svg className="h-5 w-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-4.618 1.04A11.955 11.955 0 012.944 12H2.944a11.955 11.955 0 004.618 9.056A11.955 11.955 0 0112 21.056a11.955 11.955 0 014.618-9.056A11.955 11.955 0 0121.056 12A11.955 11.955 0 0121.056 12a11.955 11.955 0 01-4.618-9.056z" />
              </svg>
            </div>
            <div>
              <h2 className="text-lg font-semibold text-gray-900">Client ID Verification</h2>
              <p className="text-sm text-gray-600">Enter client identification details below</p>
            </div>
          </div>
        </div>
        <div className="p-6">
          <form onSubmit={handleVerification} className="space-y-6">
            <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
              <div>
                <label htmlFor="idType" className="block text-sm font-medium text-gray-700">
                  ID Type
                </label>
                <select
                  id="idType"
                  value={idType}
                  onChange={(e) => setIdType(e.target.value)}
                  className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  disabled={loading.isLoading}
                >
                  <option value="NationalID">National ID</option>
                  <option value="Passport">Passport</option>
                  <option value="DriversLicense">Driver's License</option>
                </select>
              </div>
              <div>
                <label htmlFor="idNumber" className="block text-sm font-medium text-gray-700">
                  ID Number
                </label>
                <input
                  type="text"
                  id="idNumber"
                  value={idNumber}
                  onChange={(e) => setIdNumber(e.target.value)}
                  className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                  placeholder="Enter ID number"
                  disabled={loading.isLoading}
                  required
                />
              </div>
            </div>

            {error.hasError && (
              <div className="rounded-md bg-red-50 p-4">
                <div className="text-sm text-red-700">{error.message}</div>
              </div>
            )}

            <div className="flex space-x-4">
              <button
                type="submit"
                disabled={loading.isLoading || !idNumber.trim()}
                className="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {loading.isLoading && loading.message?.includes('Verifying') ? (
                  <>
                    <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" fill="none" viewBox="0 0 24 24">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Verifying...
                  </>
                ) : (
                  '🔍 Verify ID'
                )}
              </button>
              
              <button
                type="button"
                onClick={handleSearch}
                disabled={loading.isLoading || !idNumber.trim()}
                className="inline-flex justify-center py-2 px-4 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {loading.isLoading && loading.message?.includes('Searching') ? (
                  <>
                    <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-gray-700" fill="none" viewBox="0 0 24 24">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Searching...
                  </>
                ) : (
                  '🔎 Search Client'
                )}
              </button>
            </div>

            <div className="text-sm text-gray-500">
              <p className="font-medium">Demo ID Numbers:</p>
              <div className="mt-1 space-y-1">
                <p>• <span className="font-mono">150585/10/1</span> - John Banda (Verified)</p>
                <p>• <span className="font-mono">220390/10/7</span> - Mary Phiri (Verified)</p>
              </div>
            </div>
          </form>
        </div>
      </div>

      {/* Verification Results */}
      {verificationResult && (
        <div className="mt-8 bg-white shadow rounded-lg">
          <div className="px-6 py-4 border-b border-gray-200">
            <h2 className="text-lg font-medium text-gray-900">Verification Results</h2>
          </div>
          <div className="p-6">
            <div className={`rounded-md p-4 mb-6 ${verificationResult.isVerified ? 'bg-green-50' : 'bg-red-50'}`}>
              <div className="flex">
                <div className="flex-shrink-0">
                  {verificationResult.isVerified ? (
                    <svg className="h-5 w-5 text-green-400" fill="currentColor" viewBox="0 0 20 20">
                      <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
                    </svg>
                  ) : (
                    <svg className="h-5 w-5 text-red-400" fill="currentColor" viewBox="0 0 20 20">
                      <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                    </svg>
                  )}
                </div>
                <div className="ml-3">
                  <h3 className={`text-sm font-medium ${verificationResult.isVerified ? 'text-green-800' : 'text-red-800'}`}>
                    {verificationResult.isVerified ? 'ID Verified Successfully' : 'ID Verification Failed'}
                  </h3>
                  <div className={`mt-1 text-sm ${verificationResult.isVerified ? 'text-green-700' : 'text-red-700'}`}>
                    <p>Verification ID: {verificationResult.verificationId}</p>
                    <p>Source: {verificationResult.source}</p>
                    <p>Timestamp: {new Date(verificationResult.timestamp).toLocaleString()}</p>
                  </div>
                </div>
              </div>
            </div>

            {verificationResult.clientData && (
              <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
                <div>
                  <h4 className="text-sm font-medium text-gray-900 mb-3">Personal Information</h4>
                  <dl className="space-y-2">
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Full Name</dt>
                      <dd className="text-sm text-gray-900">{verificationResult.clientData.fullName}</dd>
                    </div>
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Date of Birth</dt>
                      <dd className="text-sm text-gray-900">
                        {new Date(verificationResult.clientData.dateOfBirth).toLocaleDateString()}
                      </dd>
                    </div>
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Gender</dt>
                      <dd className="text-sm text-gray-900">{verificationResult.clientData.gender}</dd>
                    </div>
                    <div>
                      <dt className="text-sm font-medium text-gray-500">ID Number</dt>
                      <dd className="text-sm text-gray-900 font-mono">{verificationResult.clientData.idNumber}</dd>
                    </div>
                  </dl>
                </div>
                <div>
                  <h4 className="text-sm font-medium text-gray-900 mb-3">Contact Information</h4>
                  <dl className="space-y-2">
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Mobile Number</dt>
                      <dd className="text-sm text-gray-900">{verificationResult.clientData.mobileNumber}</dd>
                    </div>
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Province</dt>
                      <dd className="text-sm text-gray-900">{verificationResult.clientData.province}</dd>
                    </div>
                    <div>
                      <dt className="text-sm font-medium text-gray-500">District</dt>
                      <dd className="text-sm text-gray-900">{verificationResult.clientData.district}</dd>
                    </div>
                    <div>
                      <dt className="text-sm font-medium text-gray-500">Postal Code</dt>
                      <dd className="text-sm text-gray-900">{verificationResult.clientData.postalCode}</dd>
                    </div>
                  </dl>
                </div>
              </div>
            )}

            {verificationResult.isVerified && verificationResult.clientData && (
              <div className="mt-6">
                <button
                  onClick={() => {
                    // Navigate to registration with pre-filled data
                    const searchParams = new URLSearchParams({
                      idNumber: verificationResult.clientData!.idNumber,
                      fullName: verificationResult.clientData!.fullName,
                      dateOfBirth: verificationResult.clientData!.dateOfBirth,
                      gender: verificationResult.clientData!.gender,
                      mobileNumber: verificationResult.clientData!.mobileNumber,
                      province: verificationResult.clientData!.province,
                      district: verificationResult.clientData!.district,
                      postalCode: verificationResult.clientData!.postalCode,
                    });
                    window.location.href = `/registration?${searchParams.toString()}`;
                  }}
                  className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
                >
                  👤 Proceed to Registration
                </button>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
};