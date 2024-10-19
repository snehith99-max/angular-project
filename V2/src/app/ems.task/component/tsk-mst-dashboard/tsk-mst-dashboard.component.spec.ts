import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstDashboardComponent } from './tsk-mst-dashboard.component';

describe('TskMstDashboardComponent', () => {
  let component: TskMstDashboardComponent;
  let fixture: ComponentFixture<TskMstDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstDashboardComponent]
    });
    fixture = TestBed.createComponent(TskMstDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
