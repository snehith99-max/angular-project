import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnGstoutsummaryComponent } from './acc-trn-gstoutsummary.component';

describe('AccTrnGstoutsummaryComponent', () => {
  let component: AccTrnGstoutsummaryComponent;
  let fixture: ComponentFixture<AccTrnGstoutsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnGstoutsummaryComponent]
    });
    fixture = TestBed.createComponent(AccTrnGstoutsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
