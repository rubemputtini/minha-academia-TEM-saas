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

  // Usuário autenticado (aluno)
  dashboard: "/dashboard",
  account: "/conta",
  app: "/app",
  equipments: "/app/equipamentos",
  support: "/suporte",

  // Usuário autenticado (treinador)
  coachDashboard: "/treinador/dashboard",
  coachAccount: "/treinador/conta",
  coachSubscription: "/treinador/assinatura",
  coachReferral: "/treinador/indicacao",
  coachUsers: "/treinador/alunos",
  coachClientDetail: "/treinador/alunos/:clientId",
  coachClientEquipments: "/treinador/alunos/:clientId/equipamentos",
  coachSupport: "/treinador/suporte",
  coachEquipments: "/treinador/equipamentos",

  // Rotas restritas a admin
  adminDashboard: "/admin",
  adminCoaches: "/admin/treinadores",
  adminUsers: "/admin/alunos",
  adminBaseEquipments: "/admin/equipamentos-base",
  adminSupport: "/admin/suporte",

  // Rota fallback
  fallback: "*",
};
