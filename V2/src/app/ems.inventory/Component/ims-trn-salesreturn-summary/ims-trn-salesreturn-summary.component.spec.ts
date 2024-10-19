import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnSalesreturnSummaryComponent } from './ims-trn-salesreturn-summary.component';

describe('ImsTrnSalesreturnSummaryComponent', () => {
  let component: ImsTrnSalesreturnSummaryComponent;
  let fixture: ComponentFixture<ImsTrnSalesreturnSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnSalesreturnSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnSalesreturnSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
