export const ROUTES = {
    // Públicas
    home: "/",
    signup: "/cadastro",
    coachSignup: "/cadastro/treinador",
    coachAfterPayment: "/cadastro/treinador/pos-pagamento",
    userSignup: "/cadastro/aluno",
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
  