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
    app: "/app",
    equipments: "/app/equipamentos",
  
    // Rotas restritas a admin
    dashboardAdmin: "/admin",
    usersAdmin: "/admin/alunos",
    supportAdmin: "/admin/suporte",
    
    // Rota fallback
    fallback: "*"
  };
  