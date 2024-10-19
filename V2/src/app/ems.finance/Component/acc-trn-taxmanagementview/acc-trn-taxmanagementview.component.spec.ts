import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnTaxmanagementviewComponent } from './acc-trn-taxmanagementview.component';

describe('AccTrnTaxmanagementviewComponent', () => {
  let component: AccTrnTaxmanagementviewComponent;
  let fixture: ComponentFixture<AccTrnTaxmanagementviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnTaxmanagementviewComponent]
    });
    fixture = TestBed.createComponent(AccTrnTaxmanagementviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
