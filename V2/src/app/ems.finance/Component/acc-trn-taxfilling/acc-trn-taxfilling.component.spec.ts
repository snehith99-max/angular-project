import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnTaxfillingComponent } from './acc-trn-taxfilling.component';

describe('AccTrnTaxfillingComponent', () => {
  let component: AccTrnTaxfillingComponent;
  let fixture: ComponentFixture<AccTrnTaxfillingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnTaxfillingComponent]
    });
    fixture = TestBed.createComponent(AccTrnTaxfillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
