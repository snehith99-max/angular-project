import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnGstfillingsummaryComponent } from './acc-trn-gstfillingsummary.component';

describe('AccTrnGstfillingsummaryComponent', () => {
  let component: AccTrnGstfillingsummaryComponent;
  let fixture: ComponentFixture<AccTrnGstfillingsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnGstfillingsummaryComponent]
    });
    fixture = TestBed.createComponent(AccTrnGstfillingsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
