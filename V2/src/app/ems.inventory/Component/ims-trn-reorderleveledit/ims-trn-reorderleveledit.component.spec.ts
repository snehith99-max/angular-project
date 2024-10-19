import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnReorderleveleditComponent } from './ims-trn-reorderleveledit.component';

describe('ImsTrnReorderleveleditComponent', () => {
  let component: ImsTrnReorderleveleditComponent;
  let fixture: ComponentFixture<ImsTrnReorderleveleditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnReorderleveleditComponent]
    });
    fixture = TestBed.createComponent(ImsTrnReorderleveleditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
