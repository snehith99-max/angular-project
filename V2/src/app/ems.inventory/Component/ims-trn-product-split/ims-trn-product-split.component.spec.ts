import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnProductSplitComponent } from './ims-trn-product-split.component';

describe('ImsTrnProductSplitComponent', () => {
  let component: ImsTrnProductSplitComponent;
  let fixture: ComponentFixture<ImsTrnProductSplitComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnProductSplitComponent]
    });
    fixture = TestBed.createComponent(ImsTrnProductSplitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
