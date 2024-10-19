import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnGstinsummaryComponent } from './acc-trn-gstinsummary.component';

describe('AccTrnGstinsummaryComponent', () => {
  let component: AccTrnGstinsummaryComponent;
  let fixture: ComponentFixture<AccTrnGstinsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnGstinsummaryComponent]
    });
    fixture = TestBed.createComponent(AccTrnGstinsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
