```mermaid
erDiagram
    %% Define Entities and Their Attributes
    AUTHORS {
      int ID PK
      string Bio
      binary profile_picture
      string Username
      string Email
      string Password
      datetime CreatedAt
      bool isActive
    }
    
    POSTS {
      int ID PK
      int UserID FK
      post_status status
      binary image
      string Content
      datetime CreatedAt
    }
    
    COMMENTS {
      int ID PK
      int PostID FK
      int UserID FK
      string Content
      datetime CreatedAt
    }

    REPLIES {
      int ID PK
      int CommentID FK
      string Content
      datetime CreatedAt
    }

    FRIENDSHIPS {
      int User2ID FK
      int User1ID FK
      datetime CreatedAt
    }
    
    LIKES {
      int ID PK
      int UserID FK
      int PostID FK
      datetime CreatedAt
    }
    
    
    %% Define Relationships
    AUTHORS ||--o{ POSTS: "creates"
    POSTS ||--o{ COMMENTS: "has"
    AUTHORS ||--o{ COMMENTS: "makes"
    AUTHORS ||--o{ LIKES: "likes"
    POSTS ||--o{ LIKES: "liked_by"
    AUTHORS ||--o{ FRIENDSHIPS: "is a friend of"
    COMMENTS ||--o{ REPLIES: "has"
```

<!--     <!-- GROUPS {
      int ID PK
      string GroupName
      string Description
      datetime CreatedAt
    }

    GROUP_MEMBERS {
      int GroupID FK
      int UserID FK
      datetime JoinedAt
    } --> -->