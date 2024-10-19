import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStorerequisitionviewComponent } from './ims-trn-storerequisitionview.component';

describe('ImsTrnStorerequisitionviewComponent', () => {
  let component: ImsTrnStorerequisitionviewComponent;
  let fixture: ComponentFixture<ImsTrnStorerequisitionviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStorerequisitionviewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStorerequisitionviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
