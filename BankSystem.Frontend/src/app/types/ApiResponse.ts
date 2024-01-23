type ApiResponse<T> = {
  data?: T;
  message: string;
  success: boolean;
};

export default ApiResponse;
