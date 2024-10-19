import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnSalesHistoryComponent } from './ims-trn-sales-history.component';

describe('ImsTrnSalesHistoryComponent', () => {
  let component: ImsTrnSalesHistoryComponent;
  let fixture: ComponentFixture<ImsTrnSalesHistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnSalesHistoryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnSalesHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
