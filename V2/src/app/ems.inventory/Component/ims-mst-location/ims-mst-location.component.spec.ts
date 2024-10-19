import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstLocationComponent } from './ims-mst-location.component';

describe('ImsMstLocationComponent', () => {
  let component: ImsMstLocationComponent;
  let fixture: ComponentFixture<ImsMstLocationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstLocationComponent]
    });
    fixture = TestBed.createComponent(ImsMstLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
