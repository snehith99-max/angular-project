import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMrpriceassignSummaryComponent } from './ims-trn-mrpriceassign-summary.component';

describe('ImsTrnMrpriceassignSummaryComponent', () => {
  let component: ImsTrnMrpriceassignSummaryComponent;
  let fixture: ComponentFixture<ImsTrnMrpriceassignSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMrpriceassignSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMrpriceassignSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
