import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskTrnTaskSheetDashboardComponent } from './tsk-trn-task-sheet-dashboard.component';

describe('TskTrnTaskSheetDashboardComponent', () => {
  let component: TskTrnTaskSheetDashboardComponent;
  let fixture: ComponentFixture<TskTrnTaskSheetDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskTrnTaskSheetDashboardComponent]
    });
    fixture = TestBed.createComponent(TskTrnTaskSheetDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
