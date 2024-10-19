import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnSalesHistoryviewComponent } from './ims-trn-sales-historyview.component';

describe('ImsTrnSalesHistoryviewComponent', () => {
  let component: ImsTrnSalesHistoryviewComponent;
  let fixture: ComponentFixture<ImsTrnSalesHistoryviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnSalesHistoryviewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnSalesHistoryviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
