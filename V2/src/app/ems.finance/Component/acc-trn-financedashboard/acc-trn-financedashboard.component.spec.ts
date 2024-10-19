import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnFinancedashboardComponent } from './acc-trn-financedashboard.component';

describe('AccTrnFinancedashboardComponent', () => {
  let component: AccTrnFinancedashboardComponent;
  let fixture: ComponentFixture<AccTrnFinancedashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnFinancedashboardComponent]
    });
    fixture = TestBed.createComponent(AccTrnFinancedashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
