import { Injectable } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { KeycloakAuthorizationService } from './keycloakAuthorization.service';

@Injectable({
  providedIn: 'root',
})

export class SecurityService {
  private readonly resource = "Keycloak";

  constructor(private keycloak: KeycloakService, private keycloakAuthorization: KeycloakAuthorizationService) { }

  public isUser() {
    return this.keycloak.isUserInRole('user', this.resource);
  }

  public async canAccess(): Promise<boolean> {
    if (!(await this.keycloak.isLoggedIn())) {
      return false;
    }

    try {
      return await this.keycloakAuthorization.hasEntitlement(this.resource, "Posts", "Unpublish");
    } catch (error) {
      return false;
    }
  }
}
