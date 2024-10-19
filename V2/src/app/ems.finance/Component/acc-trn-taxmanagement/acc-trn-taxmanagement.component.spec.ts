import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnTaxmanagementComponent } from './acc-trn-taxmanagement.component';

describe('AccTrnTaxmanagementComponent', () => {
  let component: AccTrnTaxmanagementComponent;
  let fixture: ComponentFixture<AccTrnTaxmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnTaxmanagementComponent]
    });
    fixture = TestBed.createComponent(AccTrnTaxmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
