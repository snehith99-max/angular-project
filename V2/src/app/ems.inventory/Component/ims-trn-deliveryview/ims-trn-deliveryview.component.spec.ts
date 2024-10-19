import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnDeliveryviewComponent } from './ims-trn-deliveryview.component';

describe('ImsTrnDeliveryviewComponent', () => {
  let component: ImsTrnDeliveryviewComponent;
  let fixture: ComponentFixture<ImsTrnDeliveryviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnDeliveryviewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnDeliveryviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
