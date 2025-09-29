/**
 * Interface padrão para respostas da API
 * Representa o formato de wrapper usado pelo backend
 */
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
  timestamp: string;
}

/**
 * Interface para respostas de sucesso simplificadas
 */
export interface ApiSuccessResponse<T> {
  success: true;
  message: string;
  data: T;
  errors: never[];
  timestamp: string;
}

/**
 * Interface para respostas de erro
 */
export interface ApiErrorResponse {
  success: false;
  message: string;
  data: null;
  errors: string[];
  timestamp: string;
}
