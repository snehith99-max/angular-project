import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnWarrantytrackerComponent } from './ims-trn-warrantytracker.component';

describe('ImsTrnWarrantytrackerComponent', () => {
  let component: ImsTrnWarrantytrackerComponent;
  let fixture: ComponentFixture<ImsTrnWarrantytrackerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnWarrantytrackerComponent]
    });
    fixture = TestBed.createComponent(ImsTrnWarrantytrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
