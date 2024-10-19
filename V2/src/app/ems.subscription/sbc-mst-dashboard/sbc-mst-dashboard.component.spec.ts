import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstDashboardComponent } from './sbc-mst-dashboard.component';

describe('SbcMstDashboardComponent', () => {
  let component: SbcMstDashboardComponent;
  let fixture: ComponentFixture<SbcMstDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstDashboardComponent]
    });
    fixture = TestBed.createComponent(SbcMstDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
