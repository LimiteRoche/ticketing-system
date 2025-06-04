export interface Ticket {
  id: string;
  userId: string;
  username: string;
  subject: string;
  description: string;
  avatarUrl: string;
  status: string;
  createdAt: string;
  replies: Reply[];
}

export interface Reply {
  id: string;
  message: string;
  agentName: string;
  avatarUrl: string;
  createdAt: string;
}

export interface TicketSummary {
  id: string;
  subject: string;
  description: string;
  avatarUrl: string;
  username: string;
  createdAt: string;
  replies: Reply[];
  status: string;
}

export interface CreateTicketRequest {
  userId: string;
  username: string;
  subject: string;
  avatarUrl: string;
  description: string;
}

export interface AddReplyRequest {
  message: string;
  agentName: string;
  avatarUrl: string;
}
