export const ROUTES = {
    // Públicas
    home: "/",
    coachSignup: "/treinador/signup",
    signup: "/signup",
    login: "/login",
    forgotPassword: "/esqueci-senha",
    resetPassword: "/redefinir-senha",
  
    // Usuário autenticado
    dashboard: "/dashboard",
    account: "/conta",
  
    // Rotas restritas a admin
    adminHome: "/admin",  
    
    // Rota fallback
    fallback: "*"
  };
  