import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnFinanceregulationComponent } from './acc-trn-financeregulation.component';

describe('AccTrnFinanceregulationComponent', () => {
  let component: AccTrnFinanceregulationComponent;
  let fixture: ComponentFixture<AccTrnFinanceregulationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnFinanceregulationComponent]
    });
    fixture = TestBed.createComponent(AccTrnFinanceregulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
