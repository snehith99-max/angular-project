import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStockregularizationComponent } from './ims-trn-stockregularization.component';

describe('ImsTrnStockregularizationComponent', () => {
  let component: ImsTrnStockregularizationComponent;
  let fixture: ComponentFixture<ImsTrnStockregularizationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStockregularizationComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStockregularizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
