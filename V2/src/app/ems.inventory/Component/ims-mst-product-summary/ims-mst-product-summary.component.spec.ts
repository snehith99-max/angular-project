import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstProductSummaryComponent } from './ims-mst-product-summary.component';

describe('ImsMstProductSummaryComponent', () => {
  let component: ImsMstProductSummaryComponent;
  let fixture: ComponentFixture<ImsMstProductSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstProductSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsMstProductSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
