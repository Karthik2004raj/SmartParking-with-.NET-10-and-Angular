import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  stats: any = null;
  zones: any[] = [];
  loading = true;

  constructor(public authService: AuthService, private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getStats().subscribe(data => {
      this.stats = data;
      this.loading = false;
    });
    
    this.dashboardService.getZones().subscribe(data => {
      this.zones = data;
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
